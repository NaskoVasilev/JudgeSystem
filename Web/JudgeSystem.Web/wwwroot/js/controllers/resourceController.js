//Delete course
$(".resourceDeleteBtn").on('click', (e) => {
	let button = $(e.target)[0];
	let courseId = button.dataset.id;
	console.log(courseId);

	$.post('/Administration/Resource/Delete', { id: courseId })
		.done((response) => {
			$(button.parentElement.parentElement).hide();
			showInfo(response);
		})
		.fail((error) => {
			showError(error.responseText);
		});
});