--
-- features.lua
--
-- $Id: features.lua,v 2d3193e43155 2019/01/21 22:17:53 keith $
--
-- Lua 5.2 module providing a mingw-get setup hook for configuration of
-- the user's MinGW GCC compiler <features.h> preferences.
--
-- Written by Keith Marshall <keithmarshall@users.sourceforge.net>
-- Copyright (C) 2019, MinGW.org Project
--
--
-- Permission is hereby granted, free of charge, to any person obtaining a
-- copy of this software and associated documentation files (the "Software"),
-- to deal in the Software without restriction, including without limitation
-- the rights to use, copy, modify, merge, publish, distribute, sublicense,
-- and/or sell copies of the Software, and to permit persons to whom the
-- Software is furnished to do so, subject to the following conditions:
--
-- The above copyright notice and this permission notice shall be included
-- in all copies or substantial portions of the Software.
--
-- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
-- OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
-- FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL
-- THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
-- LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
-- FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
-- DEALINGS IN THE SOFTWARE.
--
--
-- We begin by initializing a container, for construction of a Lua module
-- to encapsulate the content of this source file.
--
   local M = {}
--
-- mingw-get passes the MinGW installation root directory path, in the
-- $MINGW32_SYSROOT environment variable; from this, we deduce the path
-- name for the working copy of the features.h file...
--
   local function syspath( varname )
--
--   ...using this local helper function to ensure that the path name
--   string, returned from the environment, is free from insignificant
--   trailing directory name separators, and that all internal sequences
--   of directory name separators are normalized to a single '/'.
--
     local pathname = os.getenv( varname )
     if pathname
     then
       pathname = string.gsub( pathname, "[/\\]+", "/" )
       pathname = string.match( pathname, "(.*[^/])/*$" )
     end
     return pathname
   end
   local sysroot = syspath( "MINGW32_SYSROOT" )
   if not sysroot
   then
     error( "environment variable MINGW32_SYSROOT may not be set", 0 )
   end
   local config_file_name = sysroot .. "/include/features.h"
--
-- Define a template, whence the default features configuration may be
-- deduced when writing the initial content of the <features.h> file.
--
   local config_default =
   { '/*',
     ' * features.h',
     ' *',
     ' * Features configuration for MinGW.org GCC implementation; users may',
     ' * customize this file, to establish their preferred default behaviour.',
     ' * Projects may provide an alternative, package-specific configuration,',
     ' * either by placing their own customized <features.h> in the package',
     ' * -I path, ahead of the system default, or by assignment of their',
     ' * preferred alternative to the _MINGW_FEATURES_HEADER macro.',
     ' *',
     ' *',
     ' * $'..'Id$',
     ' *',
     ' * Template written by Keith Marshall <keith@users.osdn.me>',
     ' * Copyright (C) 2019, MinGW.org Project.',
     ' *',
     ' *',
     ' * Permission is hereby granted, free of charge, to any person obtaining a',
     ' * copy of this software and associated documentation files (the "Software"),',
     ' * to deal in the Software without restriction, including without limitation',
     ' * the rights to use, copy, modify, merge, publish, distribute, sublicense,',
     ' * and/or sell copies of the Software, and to permit persons to whom the',
     ' * Software is furnished to do so, subject to the following conditions:',
     ' *',
     ' * The above copyright notice, this permission notice, and the following',
     ' * disclaimer shall be included in all copies or substantial portions of',
     ' * the Software.',
     ' *',
     ' * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS',
     ' * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,',
     ' * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL',
     ' * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER',
     ' * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING',
     ' * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OF OR OTHER',
     ' * DEALINGS IN THE SOFTWARE.',
     ' *',
     ' */',
     '#ifndef __MINGW_FEATURES__',
     '#pragma GCC system_header',
     '',
     '/* Users are expected to customize this header, but it remains subject to',
     ' * automatic updates by system software.  To ensure that any customisation',
     ' * is not ovewritten, during such updates, it MUST observe the following:',
     ' *',
     ' * This header MUST define __MINGW_FEATURES__; the definition MUST begin',
     ' * with "#define __MINGW_FEATURES__ (__MINGW_FEATURES_BEGIN__) \\"; it MUST',
     ' * extend over multiple lines, and terminate with "__MINGW_FEATURES_END__";',
     ' * intervening lines may enumerate any defined features, one per line, and',
     ' * each specified as an argument to either the __MINGW_FEATURE_ENABLE__(),',
     ' * or the __MINGW_FEATURE_IGNORE__() macro, (ensuring that at least one',
     ' * space separates either of these macro names from its parenthesized',
     ' * argument name).',
     ' *',
     ' * CAUTION:',
     ' * If customizing this features configuration, ALWAYS refer to features',
     ' * using their designated symbolic constant names; NEVER usurp the use of',
     ' * these symbolic constants for any other purpose, and NEVER assume that',
     ' * any such constant has a specific value ... their definitions may vary',
     ' * between distinct MinGW Runtime Library software releases!',
     ' */',
     '#define __MINGW_FEATURES__    (__MINGW_FEATURES_BEGIN__)        \\',
     ' __MINGW_FEATURE_IGNORE__     (__MINGW_ANSI_STDIO__)            \\',
     ' __MINGW_FEATURE_IGNORE__     (__MINGW_LC_MESSAGES__)           \\',
     ' __MINGW_FEATURE_IGNORE__     (__MINGW_LC_ENVVARS__)            \\',
     ' __MINGW_FEATURES_END__',
     '',
     '#endif	/* !__MINGW_FEATURES__: $'..'RCSfile$: end of file */'
   }
--
   local function defines( name )
--
--   A local helper function, to generate a string.match
--   pattern for identification of any C #define statement,
--   which defines a specified symbol "name"
--
     return "^%s*#%s*define%s+" .. name
   end
--
   local function feature( value )
--
--   A local helper function, to generate a string.match
--   pattern for identification of a specified parenthesized
--   value field, within a C #define statement.
--
     return "%s+%(%s*" .. value .. "%s*%)%s*"
   end
--
   local function begins_definition( line, name, value )
--
--   A local helper function to check whether any input "line"
--   represents the first of a multiline C #define for the "name"
--   symbol, with its initial field matching the parenthesized
--   "value" token (default: "__MINGW_FEATURES_BEGIN__").
--
     if not value then value = "__MINGW_FEATURES_BEGIN__" end
     return string.match( line, defines( name ) .. feature( value ) .. "\\$" )
   end
--
-- In the event that a features configuration has already been
-- specified for this installation, capture this into internal
-- "as built" configuration tables...
--
   local current_config, current_features
   local config = io.open( config_file_name )
   if config
   then
--   ...always starting collection into table "current_config"...
--
     current_config = {}
     local active_list = current_config
--
--   ...reading the existing configuration file, line by line...
--
     for line in config:lines()
     do if active_list == current_features
        and string.match( line, "^%s*__MINGW_FEATURES_END__%s*$" )
        then
--	  ...noting that actual configuration options will have
--	  been diverted to the "current_features" table; when all
--	  such options have been captured, redirect any residual
--	  content to the "current_config" table.
--
	  active_list = current_config
	end
--
--	Capture the current configuration record, into whichever
--	diversion is currently active.
--
	table.insert( active_list, line )
--
	if begins_definition( line, "__MINGW_FEATURES__" )
	then
--	  When we find the first line of a (possibly) well-formed
--	  features configuration, divert capture of its subsequent
--	  lines to a new "current_features" table.
--
	  current_features = {}
	  active_list = current_features
	end
     end
--
--   When capture is complete, ensure that the input stream file
--   is closed; (we may want to reopen it later, to rewrite it).
--
     io.close( config )
--
--   Before we go any further, check for well-formedness of the
--   configuration which we have just captured; effectively...
--
     if not current_features
     or active_list == current_features
     then
--     ...when no "current_features" diversion has been created,
--     or such a diversion remains open for capture, then an error
--     has occurred, and the configuration is not well-formed.
--
       local function config_error( reason )
	 error( config_file_name .." ".. reason, 0 )
       end
       for ref, line in next, current_config
       do if string.match( line, defines( "__MINGW_FEATURES__" ) )
	  then
--	    In this case, a "__MINGW_FEATURES__" definition is
--	    present, but it is not well-formed; (note that, here,
--	    we rescan the "current_config" table, using a less
--	    rigorous criterion for detection of a definition of
--	    of "__MINGW_FEATURES__" than that which is required
--	    to open the "current_features" diversion)...
--
	    config_error( "has malformed __MINGW_FEATURES__ definition" )
	  end
       end
--     ...while in this case, no "__MINGW_FEATURES__" definition
--     was found, (well-formed, or otherwise).
--
       config_error( "does not define __MINGW_FEATURES__" )
     end
   end
--
--
   local function update_configuration( stream_file, template, current )
--
--   A function to write a features configuration to a designated output
--   stream, based on a specified template, reproducing and encapsulating
--   any existing configuration which may also have been specified...
--
     local function stream_file_writeln( line )
--
--     ...using this helper function to write each line.
--
       stream_file:write( line .. "\n" )
     end
--
     if current
     then
--     An existing configuration header is to be updated.  An image of
--     its original content has already been loaded into "current"; copy
--     it back to the original file, line by line...
--
       for ref, line in next, current
       do if begins_definition( line, "__MINGW_FEATURES__" )
	  then
--	    ...until we reach the first active configuration record.
--	    We now wish to merge any new configuration options from the
--	    template, into the existing configuration; work through the
--	    template, line by line, ignoring all lines...
--
	    local merge = false
	    for ref, sub in next, template
	    do if begins_definition( sub, "__MINGW_FEATURES__" )
	       then
--		 ...until we find the first line of a (possibly)
--		 well-formed features configuration record; when we
--		 find this, we decompose and reassemble it, so that
--		 we may preserve a tabulation format matching the
--		 longer of the template and actual configuration
--		 records.
--
		 local def = string.match( sub, "^[^(]*" )
		 local usr = string.match( line, "^[^(]*" )
		 if string.len( usr ) > string.len( def )
		 then
		   def = usr
		 end
		 usr = string.match( line, "%(.*" )
		 sub = string.match( sub, "%(.*" )
		 if string.len( usr ) > string.len( sub )
		 then
		   sub = usr
		 end
--
--		 Write out the reassembled features configuration
--		 start record, and activate the features merge.
--
		 stream_file_writeln( def .. sub )
		 merge = true
--
--	       During the merge operation, when we find the record
--	       terminator within the template...
--
	       elseif string.match( sub, "^%s*__MINGW_FEATURES_END__%s*$" )
	       then
--		 ...we immediately write out any additional option
--		 specifications, which remain in the user specified
--		 configuration...
--
		 for ref, sub in next, current_features
		 do
		   stream_file_writeln( sub )
		 end
--
--		 ...and break out of the merge cycle.
--
		 break
--
	       elseif merge
	       then
--	         While we remain within the merge cycle, we extract
--	         the identification of each configuration option from
--	         the template, comparing it with all entries in the
--	         "current_features" table...
--
		 tag = string.match( sub, "%(.*%)" )
		 for ref, usr in next, current_features
		 do if string.match( usr, tag )
		    then
--
--		      ...and, when we find a match, we favour the
--		      existing configuration record over that from
--		      the template...
--
		      sub = usr
--
--		      ...remove the corresponding entry from the
--		      "current_features" table, and break out of
--		      the inner matching loop, before...
--
		      table.remove( current_features, ref )
		      break
		    end
		 end
--
--		 ...in either event, writing out a copy of each
--		 selected configuration record.
--
		 stream_file_writeln( sub )
	       end
	    end
	  else
--
--	    For every line in the existing configuration file, outside
--	    the scope of the "__MINGW_FEATURES__" definition itself, we
--	    simply write out a verbatim copy of each line.
--
	    stream_file_writeln( line )
	  end
       end
     else
--
--     There is no existing configuration header, so we simply reproduce
--     the template, processing it line by line...
--
       for ref, line in next, template
       do
--	 ...and writing out each line individually.
--
	 stream_file_writeln( line )
       end
     end
   end
--
--
   function M.pathname( suffix )
--
--   An exported utility function, to facilitate identification of
--   the full MS-Windows path name for the features.h configuration
--   file, as appropriate to the current installation...
--
     if suffix
     then
--     ...appending any suffix which may have been specified, (e.g.
--     to specify a reference to the features.h.sample file)...
--
       return config_file_name .. suffix
     end
--
--   ...otherwise, specifying a reference to features.h itself.
--
     return config_file_name
   end
--
--
   function M.initialize( stream_file )
--
--   Primary initialization function; overwrites any existing features
--   configuration header, opened for writing as "stream_file"...
--
     if not stream_file
     then
--     ...or falling back to "io.stderr", in the event that no output
--     stream file has been opened...
--
       stream_file = io.stdout
     end
--
--   ...replacing any existing configuration with an exact copy of the
--   "config_default" content specified within this module.
--
     update_configuration( stream_file, config_default )
   end
--
--
   function M.update( stream_file )
--
--   Primary API function, exported for use by mingw-get (or any other
--   client); overwrites any existing configuration header file, which
--   has been opened for writing as "stream_file"...
--
     if not stream_file
     then
--     ...or falling back to "io.stderr", in the event that no output
--     stream file has been opened...
--
       stream_file = io.stdout
     end
--
--   ...preserving any existing configuration, with the addition of
--   any configuration options which have been included in the default
--   template, but which do not yet appear in the existing header.
--
     update_configuration( stream_file, config_default, current_config )
   end
--
-- Since this source file is intended to be loaded as a Lua module, we
-- must ultimately return a reference handle for it.
--
   return M
--
-- $RCSfile: features.lua,v $: end of file */
