//Dropdown courses
var toggler = $(".rotate");

for (let i = 0; i < toggler.length; i++) {
	$(toggler[i]).click(() => {
		$(toggler[i].parentElement.querySelector(".hidden-element")).toggle("active");
		toggler[i].classList.toggle("rotate-down");
	});
}

//Delete course
$(".courseDeleteBtn").on('click', (e) => {
	let button = $(e.target)[0];
	let courseId = button.dataset.id;
	console.log(courseId);

	$.post('/Administration/Course/Delete', { id: courseId })
		.done((response) => {
			$(button.parentElement.parentElement.parentElement).hide();
			$('#course-' + courseId).hide();
			showInfo(response);
		})
		.fail((error) => {
			showError(error.responseText);
		});
});