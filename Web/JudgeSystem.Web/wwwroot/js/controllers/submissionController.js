$(".problem-name").on("click", (e) => {
	$(".active-problem").removeClass("active-problem");
	$(e.target).addClass("active-problem");
});

$('#submit-btn').on('click', () => {
	let problemId = $('.active-problem')[0].dataset.id;
	let code = editor.getValue();
	editor.setValue("");

	$.post('/Submission/Create', { problemId, code })
		.done((response) => {
			$('#submissions-holder').DataTable({
				data: response
			});
			showInfo(response);
		})
		.fail((error) => {
			showError(error.responseText);
		});
});