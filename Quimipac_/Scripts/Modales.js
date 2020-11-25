function modalAdd(id_boton, id_modal, url_modal) {
    $(id_boton).click(function (eve) {
        cargaModal(id_modal, url_modal);
    })
}

function modalEdit(element, modal, url) {
    $(element).click(function (eve) {

        var id = $(this).data('id');
        var urlNueva = url.replace("param-id", id);
        cargaModal(modal, urlNueva);
    })
}


function cargaModal(id_modal, url_modal) {
    $('#' + id_modal).load(url_modal);
}
