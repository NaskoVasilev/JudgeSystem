$(".problem-name").on("click", (e) => {
	
	let oldId = $(".active-problem")[0].dataset.id;
	$(".active-problem").removeClass("active-problem");
	$(e.target).addClass("active-problem");
	let id = $(e.target)[0].dataset.id;

	$('#problemName')[0].innerText = e.target.textContent;

	let editProblemBtnHrefAttribute = $('#editProblemBtn').attr('href');
	editProblemBtnHrefAttribute = editProblemBtnHrefAttribute.replace(`/${oldId}`, `/${id}`);
	$('#editProblemBtn').attr('href', editProblemBtnHrefAttribute);

	let deleteProblemBtnHrefAttribute = $('#deleteProblemBtn').attr('href');
	deleteProblemBtnHrefAttribute = deleteProblemBtnHrefAttribute.replace(`/${oldId}`, `/${id}`);
	$('#deleteProblemBtn').attr('href', deleteProblemBtnHrefAttribute);

	let addTestHrefAttribute = $('#addTestBtn').attr('href');
	addTestHrefAttribute = addTestHrefAttribute.replace(`problemId=${oldId}`, `problemId=${id}`);
	$('#addTestBtn').attr('href', addTestHrefAttribute);

	let allTestsBtnHrefAttribute = $('#allTestsBtn').attr('href');
	allTestsBtnHrefAttribute = allTestsBtnHrefAttribute.replace(`problemId=${oldId}`, `problemId=${id}`);
	$('#allTestsBtn').attr('href', allTestsBtnHrefAttribute);
});

$('#submit-btn').on('click', () => {
	let problemId = $('.active-problem')[0].dataset.id;
	let code = editor.getValue();
	
	editor.setValue("");

	$.post('/Submission/Create', { problemId, code })
		.done((response) => {
			console.log(response);
			//showInfo(response);
		})
		.fail((error) => {
			showError(error.responseText);
		});
});