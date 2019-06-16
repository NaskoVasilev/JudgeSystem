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
	let tr = $('<tr></tr>');
	let pointsTd = $('<td></td>');
	let spinner = $('<div class="spinner-border text-success" role="status"></div>');
	pointsTd.append(spinner);
	tr.append(pointsTd);
	let tbody = $('#submissions-holder tbody');
	tbody.prepend(tr);
	editor.setValue("");

	$.post('/Submission/Create', { problemId, code })
		.done((response) => {
			tr = $('<tr></tr>');
			pointsTd = $('<td></td>');
			$('#submissions-holder tbody tr:first-of-type').remove();

			if (!response.isCompiledSuccessfully) {
				pointsTd.text("Compile time error");
			}
			else {
				for (let test of response.executedTests) {
					if (test.isCorrect) {
						pointsTd.append('<i class="fas fa-check text-success"></i>');
					}
					else if (test.ExecutedSuccessfully) {
						pointsTd.append('<i class="fas fa-times text-danger"></i>');
					}
					else {
						pointsTd.append('<i class="fas fa-bomb text-danger"></i>');
					}
				}
			}

			tr.append(pointsTd);
			tbody.append(tr);
		})
		.fail((error) => {
			showError(error.responseText);
		});
});