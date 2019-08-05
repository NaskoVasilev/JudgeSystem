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
	let lessonId = button.dataset.id;

	$.post('/Administration/Course/Delete', { id: lessonId })
		.done((response) => {
			$(button.parentElement.parentElement.parentElement).hide();
			$('#course-' + lessonId).hide();
            showInfo(response);
            console.log(response)
		})
		.fail((error) => {
			showError(error.responseText);
            console.log(error);
		});
});