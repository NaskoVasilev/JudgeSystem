$("form").on("change", ".file-upload-field", function () {
    var files = this.files;
    let fileNames = "Select your files!";
    if (files.length > 0) {
        fileNames = Array.from(files).map(x => x.name).join(", ");
    }


    $(this).parent(".file-upload-wrapper").attr("data-text", fileNames);
    let value = this.files;
    console.log(value);
});