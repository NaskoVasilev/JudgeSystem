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

$("form").on("submit", showFileUploader);

function showFileUploader() {
    if ($(".file-upload-field")[0].files.length === 0) {
        return;
    }

    $("#resource-button").attr("disabled", "disabled");
    let loader = $('<div class="spinner-border text-success uploader ml-3" role="status"></div>');
    let container = $('<div class="loader text-success">Uploading file...</div>');
    container.append(loader);
    let fileInputDiv = $(".file-upload-wrapper").parent();
    fileInputDiv.before(container);
}