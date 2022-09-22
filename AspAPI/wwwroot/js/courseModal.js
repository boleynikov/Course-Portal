function openModal(parameters) {
     var id = parameters.data;
     var url = parameters.url;
     var modal = $('#modal');

    if (id === undefined || url === undefined) {
        alert('Data miss')
    }

    $.ajax({
        type: 'GET',
        url: url,
        data: { "id": id },
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show')
        },
        failure: function () {
            modal.modal('hide')
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};