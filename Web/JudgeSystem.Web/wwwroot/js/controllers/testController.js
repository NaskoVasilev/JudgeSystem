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

//This code get new values for test input and output and sent request to /Administration/Test/Edit to edit the test
//$(".testEditBtn").on('click', (e) => {
//	let button = $(e.target)[0];
//	let testId = button.dataset.id;

//	let inputData = testEditors.get('input-data-' + testId).getValue();
//	let outputData = testEditors.get('output-data-' + testId).getValue();

//	console.log(inputData);
//	console.log(outputData);

//	$.post('/Administration/Test/Edit', { id: testId, inputData, outputData })
//		.done((response) => {
//			showInfo(response);
//		})
//		.fail((error) => {
//			showError(error.responseText);
//		});
//});


function setTestEditors(selector) {
	let textareas = $(selector);
	let textEditor;

	for (let textarea of textareas) {
		textEditor = CodeMirror.fromTextArea(textarea,
			{
                lineNumbers: true,
                readOnly: 'cursor'
			});
		textEditor.setValue(textarea.textContent);
		textEditor.setSize("100%", "150px");

		let textareaId = textarea.id;
		testEditors.set(textareaId, textEditor);
	}
}