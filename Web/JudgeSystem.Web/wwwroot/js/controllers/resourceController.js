//Delete resource
let clickedDeleteButton;

$("#delete-confirm-btn").on('click', (e) => {

    if (clickedDeleteButton) {
        let resourceId = clickedDeleteButton.dataset.id;

        $.post('/Administration/Resource/Delete', { id: resourceId })
            .done((response) => {
                $(clickedDeleteButton.parentElement.parentElement).hide();
                showInfo(response);
            })
            .fail((error) => {
                showError(error.responseText);
            });
    }
});

$(".resourceDeleteBtn").on('click', (e) => {
    clickedDeleteButton = $(e.target)[0];
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