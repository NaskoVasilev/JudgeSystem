let testEditors = new Map();
setTestEditors('.test-data');


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

	let inputData = testEditors.get('input-data-' + testId).getValue();
	let outputData = testEditors.get('output-data-' + testId).getValue();

	console.log(inputData);
	console.log(outputData);

	$.post('/Administration/Test/Edit', { id: testId, inputData, outputData })
		.done((response) => {
			showInfo(response);
		})
		.fail((error) => {
			showError(error.responseText);
		});
});


function setTestEditors(selector) {
	let textareas = $(selector);
	let textEditor;

	for (let textarea of textareas) {
		textEditor = CodeMirror.fromTextArea(textarea,
			{
				lineNumbers: true
			});
		textEditor.setValue(textarea.textContent);
		textEditor.setSize("100%", "150px");

		let textareaId = textarea.id;
		testEditors.set(textareaId, textEditor);
	}
}