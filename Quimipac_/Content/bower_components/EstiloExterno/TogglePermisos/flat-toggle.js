$(document).ready(function () {
    $('.flat-toggle').on('click', function (e) {
        if ($(this).hasClass('on')) {
            $(this).removeClass('on');
           
            $('#Tipos de Trabajo').removeClass('on');

        } else {
            $(this).addClass('on'); 

            var id = e.target.id;
            if (id == 'Mantenimiento') {
                $('.flat-toggle-Mant').css('background-color','#8ce2ff');
             alert(id); }
          
           

        }
    });
});


 var cntSubMenu = 0;
    function mostrarSubMenu() {

        if (cntSubMenu%2 == 0) {
            document.getElementById('DivSubMenu').style.display = 'block';

            //if ($('#Tipos de Trabajo').hasClass('on')) {
            //    $('#Tipos de Trabajo').removeClass('on');
            //} else {
            //    $('#Tipos de Trabajo').addClass('on');
            //}


            //$('#Tipos de Trabajo').hasClass('on');
            //$('#Precios Referenciales').hasClass('on');
            //$('#Lugares Medición').hasClass('on');
            //$('#Grupo Trabajo').hasClass('on');
            //$('#Sector').hasClass('on');
            //$('#Servicios').hasClass('on');
            //$('#Tablas Referenciales').hasClass('on');
            //$('#Permisos').hasClass('on');
            //$('#Campamentos').hasClass('on');
        } else {
            document.getElementById('DivSubMenu').style.display = 'none';
            //$('#Tipos de Trabajo').removeClass('on');
            //$('#Precios Referenciales').removeClass('on');
            //$('#Lugares Medición').removeClass('on');
            //$('#Grupo Trabajo').removeClass('on');
            //$('#Sector').removeClass('on');
            //$('#Servicios').removeClass('on');
            //$('#Tablas Referenciales').removeClass('on');
            //$('#Permisos').removeClass('on');
            //$('#Campamentos').removeClass('on');

        }

        cntSubMenu++;
    }