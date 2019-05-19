// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var toggler = $(".rotate");

for (let i = 0; i < toggler.length; i++) {
	$(toggler[i]).click(() => {
		$(toggler[i].parentElement.querySelector(".hidden-element")).toggle("active");
		toggler[i].classList.toggle("rotate-down");
	});

	//toggler[i].addEventListener("click", function () {
	//	this.parentElement.querySelector(".nested").classList.toggle("active");
	//	this.classList.toggle("caret-down");
	//});
}