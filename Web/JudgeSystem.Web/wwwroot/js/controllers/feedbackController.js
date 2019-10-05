$(".feedback-delete-btn").on('click', (e) => {
    let button = $(e.target)[0];
    let feedbackId = button.dataset.id;

    $.post('/Administration/Feedback/Archive', { id: feedbackId })
        .done((response) => {
            $(button.parentElement.parentElement).hide();
            showInfo(response);
        })
        .fail((error) => {
            showError(error.responseText);
        });
});