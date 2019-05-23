$(".testDeleteBtn").on('click', (e) => {
	let button = $(e.target)[0];
	let testId = button.dataset.id;
	console.log(testId);

	$.post('/Administration/Test/Delete', { id: testId })
		.done((response) => {
			$(button.parentElement.parentElement).hide();
			showInfo(response);
		})
		.fail((error) => {
			showError(error.responseText);
		});
});

$(".testEditBtn").on('click', (e) => {
	let button = $(e.target)[0];
	let testId = button.dataset.id;

	let inputData = $(`#input-data-${testId}`).val();
	let outputData = $(`#output-data-${testId}`).val();

	$.post('/Administration/Test/Edit', { id: testId, inputData, outputData })
		.done((response) => {
			showInfo(response);
		})
		.fail((error) => {
			showError(error.responseText);
		});
});