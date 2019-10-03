//Dropdown courses
var toggler = $(".rotate");

for (let i = 0; i < toggler.length; i++) {
	$(toggler[i]).click(() => {
		$(toggler[i].parentElement.querySelector(".hidden-element")).toggle("active");
		toggler[i].classList.toggle("rotate-down");
	});
}