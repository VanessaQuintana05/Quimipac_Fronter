
var ArrayMTPermiso = [];
var UrlPermiso = location.href;//CadUrl+"";
var datearray1 = UrlPermiso.split("/");
var IdUser = datearray1[datearray1.length - 1];

var contTabla = 0;
var contOpcion = 0;

    $('#tabla').DataTable({

        "language": {
         //   "lengthMenu": "Display _MENU_ records per page",
            "zeroRecords": "No Contiene registros",
            "infoEmpty": "No hay registros disponibles",
            "decimal": "",
            "emptyTable": "No hay datos",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
            //"infoEmpty": "Mostrando 0 a 0 de 0 registros",
            "infoFiltered": "(Filtro de _MAX_ total registros)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "  Mostrar _MENU_ registros",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            //"zeroRecords": "No se encontraron coincidencias",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Atras"
            },
            "aria": {
                "sortAscending": ": Activar orden de columna ascendente",
                "sortDescending": ": Activar orden de columna desendente"
            },
        },


        'paging': true,
        'lengthChange': true,
        'searching': true,
        'ordering': false,
        'info': true,
        'autoWidth': false
   });

//$(document).ready(function () {

//});


$('.select2').select2({
    dropdownParent: $('#modal1')
    }); 

//});

function resultado(url1, Usuario, actEliminar) {
    //contarClients();
        var n = 0;
        var resultadoUsuario = [];// = "";
        $('.checkClientes:checked').each(function () {
        //('input[type=checkbox]:checked').ach(function () {
            var Id_Cliente = $(this).prop("id");
            var Nombre_Cliente = $(this).val();//$(this).prop("value");
            var fila = {
                Id_Cliente,
                Nombre_Cliente
            };
            //console.log("Id_Cliente:"+Id_Cliente+" Nombre_Cliente:"+Nombre_Cliente);
              //console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") Seleccionado");
            resultadoUsuario.push(fila);
            n++;
        });
        if (n > 0) {
            var url = url1;//'UrlAct"AgregarClientes_Permisos", "Mantenimiento", new { id = "param-id" });'
            url = url.replace("param-id", Usuario);

            //contAjaxTabla++;
       // alert(url);
            ejecutaAjaxtabla("tbodyClientes", Usuario, resultadoUsuario, url);
        } else {
            swal('', 'Para agregar un Cliente debe seleccionar un checkbok', 'error');
        }
//        $("#tbodyClientes").empty();
    }

    function ejecutaAjaxtabla(id,Usuario, resultado, url) {

     //   var Actioncontrato = @Url.Action("AddContratosClientes_Modal","Mantenimiento", new { id ="param-name"});//'@Url.Action("AddContratosClientes_Modal","Mantenimiento", new { id ="param-name"})';
        


        var jsonObject = { lkItems: resultado, Usuario:Usuario };// ValueTipo: ValueTipo, ValueProyCont: ValueProyCont, ValueTipoTrab: ValueTipoTrab, ValueMoneda: ValueMoneda };

        $.ajax({
            type: 'POST',
            url: url,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(jsonObject),

            success: function (data) {
                    $("#"+id).empty();
                    console.log(data);
                    $.each(data, function (i, item) {
                        $("#" + id).append("<tr onclick='ResaltaFila(this)' id='" + item.Id_Cliente + "' class='" + item.Id_Cliente + "TR' >" +
                            "<td style='display:none;'>"+item.Id_Cliente+"</td>"+
                            "<td nowrap >"+
                            "<button id='btnEliminar_" + item.Id_Cliente +"' type='button' class='btn btn-sm' value='' onclick='EliminarFila(this)' style='background:none;display:none;'>" +
                                    "<font color='#d31519' size='2'><span aria-hidden='true' class='fa fa-trash-o'></span></font>" +
                                "</button>" +// &nbsp;"+ 


                                //"<a href='@Url.Action('Eliminar_MantRolUsuario','Mantenimiento', new {id = item1.cod_cli})' target='_self' class='btn btn-sm' title='Quitar rol a usuario' style='text-align:center'>" +
                                //    "<font color='#d31519' size='2'><span aria-hidden='true' class='fa fa-trash-o'></span></font>"+
                                //"</a>" +


                              /*"<a href='@Url.Action('Eliminar_MantRolUsuario','Mantenimiento', new {id = item1.cod_cli})' target='_self' class='btn btn-sm' title='Añadir Contrato' style='text-align:center'>" +
                                    "<font color='#0d860d' size='2'><span aria-hidden='true' class='fa fa-book'></span></font>" +
                                "</a>"+
                                "<a href='@Url.Action('Eliminar_MantRolUsuario','Mantenimiento', new {id = item1.cod_cli})' target='_self' class='btn btn-sm' title='Añadir Proyectos' style='text-align:center'>" +
                                    "<font color='#3d8bba' size='2'><span aria-hidden='true' class='fa fa-briefcase'></span></font>" +
                                "</a>"+*/
/*

onclick="resultado('("AgregarClientes_Permisos", "Mantenimiento", new { id = "param-id" })','@IdUsuario','@Url.Action("Eliminar_MantRolUsuario","Mantenimiento", new {id = "param-id"})')">Crear</button>
                        */

                                "<button type='button' id='" + item.Id_Cliente + "' name='" + item.Nombre_Cliente + "' class='btn btn-sm class_Agregar_ManPermiso_Contratos' title='Agregar Contratos' style='background:none;' data-toggle='modal' data-target='#modalAgregarContratos' data-id='" + item.Id_Cliente + "' onclick= 'mostrarDivContrato(this)'  >" +//data-url='UrlAction("ClearData", "Home", new { id ="param-name" })'>"+
                                    "<font color='#0d860d' size='2'><span aria-hidden='true' class='fa fa-book'></span></font>" +
                                "</button>"+
                            
                                "<button type='button' id='" + item.Id_Cliente + "' name='" + item.Nombre_Cliente + "' class='btn btn-sm class_Agregar_ManPermiso_Proyectos' title='Agregar Proyectos' style='background:none;' data-toggle='modal' data-target='#modalAgregarProyectos' data-id='" + item.Id_Cliente + "' onclick= 'mostrarDivProyecto(this)'  >" +
                                    "<font color='#3d8bba' size='2'><span aria-hidden='true' class='fa fa-briefcase'></span></font>" +
                                "</button>"+
                            "</td>" +
                            "<td id='" + item.Id_Cliente + "TD' class='"+item.Id_Cliente+"'>" + item.Nombre_Cliente+"</td></tr>");
                    });

                    
               /* if (mensaje.indexOf('!') != -1) {
                    $("#DivDanger").css('display', 'block');
                    $("#CadenaDanger").html(mensaje);
                    $("#DivDanger").delay(4000).fadeOut(300);
                    console.log(mensaje); //alert(mensaje);
                } else {
                    $("#DivSucess").css('display', 'block');
                    $("#CadenaSucess").html(mensaje);
                    $("#DivSucess").delay(2000).fadeOut(300);
                    console.log(mensaje); //alert(mensaje);
                    location.reload();
                }*/

                    //$('#btnSalir').trigger('click');
                    document.getElementById('btnSalir').click();
                  // $('#modalAgregarCliente').modal("dispose");
            

            },
            error: function () {
                //$("#DivError").css('display', 'block');
                //$("#DivError").delay(1000).fadeOut(300);
            }

          
        });
    }

  

/*JavaScript de Agregar Contratos de acuerdo al clientes seleccionado*/
/*-------------------------------------------------------------------*/
    var cont = 0;
    var vqActiva;
    var vqAnterior;

/*function ResaltaFila(x) {
    //$('#tablaCliente tr').click(function (e) {

    if (vqActiva != x) {
        cont = 0;
        //alert(x+'   diferentw');
    }
    $('#tablaCliente tr').css('background','#ffffff');
    if (cont%2 == 0) {
        x.style.background = "#ffd799";
        vqActiva = x;
    } else {
        x.style.background = "#ffffff";//.removeClass('highlighted');

    }
   // });
        cont++;
    }*/


function ResaltaFila(x) {
    if (vqActiva != x) {
      cont = 0;
    }
    $('#tablaCliente td').css('background', '#ffffff');
    if (cont % 2 == 0) {
       
        $('#'+x.id+'TD').css('background', '#ffd799');
        vqActiva = x;
    } else {
        $('#'+x.id+'TD').css('background', '#ffffff');
    }
    cont++; //console.log(cont + '|');
}

function EliminarFila(x) {
  //  alert(x.id);
    
    var IDBoton = x.id;
    var Separador = IDBoton.split("_");
    var Id = Separador[1];
    $('.' + Id + 'TR').remove();
    //console.log(Id+'TR');
}


var VecesPaginacion = 0;
//mostrarDivContrato      mostrarDivProyecto
function mostrarDivContrato(btn) {//url1) {

    var url = urlAddContrato;//UrlAction'AddContratosClientes_Modal','Mantenimiento', new { id ='param-id'});//@Url.Action("AddContratosClientes_Modal","Mantenimiento", new { id ="param-name"});


    var nameBotonClick =$('#NombreCliente').text(btn.name);
   // alert(btn.name);
    //alert(btn.id);
    
    var UrlM = location.href;//CadUrl+"";
    var datearray1 = UrlM.split("/");
    var Usuario = datearray1[datearray1.length - 1];

    //var url = url1;
    url = url.replace("param-id", btn.id);
    //alert(id+'....'+url1);
    //var url = '@Url.Action("GetItems", "Mantenimiento", new { id = "param-id" })';
    AjaxLlenarContratos("tbodyContratos", url, Usuario, btn.id);
    //alert('ID:'+ v.id+'   v..:'+v );
   // $('#DivContrato').css("display","block");*/ 
}
function AjaxLlenarContratos(id, url, Usuario, IdClienteBoton) {


    var cont = 0;
    //alert(id);
    $.ajax({
        type: 'GET',
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        //var dataJson = JSON.parse(data),
        success: function (data) {
            $("#" + id).empty();
            console.log('Contratosgeneral:');
            console.log(data);

            $.each(data, function (i, item) {
                $("#" + id).append("<tr>"// + "<td>" + i+"</td></tr>");
                    //+  "<td nowrap> <input type='checkbox' name='SelectedTriggers' value='" + item.Id_Contrato + "' id='" + item.Id_Contrato + "'/>&nbsp;" + data[i].Nombre + "</td></tr>");
                    + "<td nowrap> <input type='checkbox' name='"+IdClienteBoton+"' value='" + data[i].Nombre + "' id='" + data[i].Id_Contrato + "' class='checkcontratos'/>&nbsp;" + data[i].Nombre + "</td></tr>");
                cont++;
            });

            //var VecesPaginacion = 0;
          
            if (VecesPaginacion >0) {
                $('#nav').remove(); 
            }
            /* * * * * * * * * 
            $('#tablaContratos').after('<div id="nav"></div>');
//           $("#nav").remove(); 
            var rowsShown = 4;//10;
            var rowsTotal = $('#tablaContratos tbody tr').length;
            //alert(rowsTotal);
            var numPages = rowsTotal / rowsShown;
            for (i = 0; i < numPages; i++) {
             //   alert(i);
                var pageNum = i + 1;
                //..Paginacion.. 
                $('#nav').append('<a href="#" rel="' + i + '">' + pageNum + '</a> ');
                VecesPaginacion++;
            }
            $('#tablaContratos tbody tr').hide();
            $('#tablaContratos tbody tr').slice(0, rowsShown).show();
            $('#nav a:first').addClass('active');
            $('#nav a').bind('click', function () {

                $('#nav a').removeClass('active');
                $(this).addClass('active');
                var currPage = $(this).attr('rel');
                var startItem = currPage * rowsShown;
                var endItem = startItem + rowsShown;
                $('#tablaContratos tbody tr').css('opacity', '0.0').hide().slice(startItem, endItem).
                    css('display', 'table-row').animate({ opacity: 1 }, 300);
            }); * * * * ** * * */
            
    /*var cont = 0;
    //alert(id);
    $.ajax({
        type: 'GET',
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id}),
        success: function (result) {
            $("#" + id).empty();
            console.log(result);
            
            $.each(data, function (i, item) {
                $("#" + id).append("<tr>"// + "<td>" + i+"</td></tr>");
                    //+  "<td nowrap> <input type='checkbox' name='SelectedTriggers' value='" + item.

            + "' id='" + item.Id_Contrato + "'/>&nbsp;" + data[i].Nombre + "</td></tr>");
                    + "<td nowrap> <input type='checkbox' name='SelectedTriggers' value='" + result[i].Id_Contrato + "' id='" + result[i].Id_Contrato + "'/>&nbsp;" + result[i].Nombre + "</td></tr>");
                cont++;                  
            });

           
    */

        //    JSONUTIL dataSet = JsonConvert.DeserializeObject<JSONUTIL>(JSONContenido);


            /*

            success: function (data) {
            $("#" + id).empty();
            console.log(data);
            $.each(data, function (i, item) {
                $("#" + id).append("<tr>"// + "<td>" + i+"</td></tr>");
                +   "<td nowrap> <input type='checkbox' name='SelectedTriggers' value='" + item.Id_Contrato + "' id='" + item.Id_Contrato + "'/>&nbsp;" + item.Nombre + "</td></tr>");
                cont++;
            });

            */
            //if (cont == 0) {
            //    $("#" + id).append("<tr><td>" +"<label for='recipient-name' class='text-bold control-label col-md-12' style='text-align:left;background-color:#fcf8e4;height:28px;'>No existen Contratos registrados del cliente.</label>"+"</tr></td>");

            //}


            /* if (mensaje.indexOf('!') != -1) {
                 $("#DivDanger").css('display', 'block');
                 $("#CadenaDanger").html(mensaje);
                 $("#DivDanger").delay(4000).fadeOut(300);
                 console.log(mensaje); //alert(mensaje);
             } else {
                 $("#DivSucess").css('display', 'block');
                 $("#CadenaSucess").html(mensaje);
                 $("#DivSucess").delay(2000).fadeOut(300);
                 console.log(mensaje); //alert(mensaje);
                 location.reload();
             }*/
            

        },
        error: function () {
            //$("#DivError").css('display', 'block');
            //$("#DivError").delay(1000).fadeOut(300);
        }


    });
}

//txt buscador
$(function () {
    $("#searchTxt").keyup(function () {
        var texto = $(this).val();

        $("#tablaContratos tr").each(function () {
            var resultadoTabla = $(this).text().toLowerCase().indexOf(texto.toLowerCase());

            if (resultadoTabla < 0) {
                $(this).fadeOut();
            }
            else {
                $(this).fadeIn();
            }
        });
    });
});
/*
$("#searchTxt").keyup(function () {
    _this = this;
    // Show only matching TR, hide rest of them
    $.each($("#tablaContratos tbody tr"), function () {
        if ($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) === -1)
            $(this).hide();
        else
            $(this).show();
    });
});
*/

//agregar a la tabla de contrato
function AgregarTablacontrato(btn) {
    //var nameBotonClick = $('#NombreCliente').text(btn.name);
    var url = urlAddContratoTabla;
    var NombreCliente = $('#NombreCliente').text();
    //alert(NombreCliente);
    var n = 0;
    var resultadoContrato = [];

    var idClienteCheck;
    //$('input[type=checkbox]:checked').each(function () {
    $('.checkcontratos:checked').each(function () {
        var Id_Contrato = $(this).prop("id");
        var Nombre = $(this).val();//$(this).prop("value");
        idClienteCheck = $(this).prop("name");
       // alert('ClassCheckCliente:' + idClienteCheck);
        var fila = {
            Id_Contrato,
            Nombre
        };
      //**** console.log("Id_Contrato:" + Id_Contrato + " Nombre:" + Nombre);
        //console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") Seleccionado");
       resultadoContrato.push(fila);
        n++;
    });
    
    if (n > 0) {
       // var url = url1;//'UrlAct"AgregarClientes_Permisos", "Mantenimiento", new { id = "param-id" });'
        ///url = url.replace("param-id", Usuario);

        // alert(url);
        ejecutaAjaxtablaContrato("tbodyContratosAdd", NombreCliente, resultadoContrato, url, idClienteCheck);//id
    } else {
        swal('', 'Para agregar un contrato debe seleccionar un checkbok', 'error');
    }


}
function ejecutaAjaxtablaContrato(id, NombreCliente, resultadoContrato, url, idClienteCheck) {

    //   var Actioncontrato = @Url.Action("AddContratosClientes_Modal","Mantenimiento", new { id ="param-name"});//'@Url.Action("AddContratosClientes_Modal","Mantenimiento", new { id ="param-name"})';



    var jsonObject = { lkItems: resultadoContrato }// Usuario: Usuario }

    $.ajax({
        type: 'POST',
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonObject),

        success: function (data) {
        //..........    $("#" + id).empty();
          ///  console.log(data);
            $.each(data, function (i, item) {//TR: id =Contrato   Name= idClienteCheck   onclick='ResaltaFila(this)'

                $("#" + id).append("<tr  id='" + item.Id_Contrato + "' class='" + item.Id_Contrato + "TR' name='" + idClienteCheck + "'>" +
                   
                    "<td style='display:none;'>"+idClienteCheck+"</td>"+
                    "<td style='display:none;'>"+item.Id_Contrato+"</td>"+
                    "<td nowrap>" +
                        "<button id='btnEliminar_" + item.Id_Contrato + "' type='button' class='btn btn-sm' value='' onclick='EliminarFilaContrato(this)' style='background:none'>" +
                        "<font color='#d31519' size='2'><span aria-hidden='true' class='fa fa-trash-o'></span></font>" +
                        "</button>" +// &nbsp;"+ 
                     "</td>"+
                    //"<a href='@Url.Action('Eliminar_MantRolUsuario','Mantenimiento', new {id = item1.cod_cli})' target='_self' class='btn btn-sm' title='Quitar rol a usuario' style='text-align:center'>" +
                    //    "<font color='#d31519' size='2'><span aria-hidden='true' class='fa fa-trash-o'></span></font>"+
                    //"</a>" +


                    /*"<a href='@Url.Action('Eliminar_MantRolUsuario','Mantenimiento', new {id = item1.cod_cli})' target='_self' class='btn btn-sm' title='Añadir Contrato' style='text-align:center'>" +
                          "<font color='#0d860d' size='2'><span aria-hidden='true' class='fa fa-book'></span></font>" +
                      "</a>"+
                      "<a href='@Url.Action('Eliminar_MantRolUsuario','Mantenimiento', new {id = item1.cod_cli})' target='_self' class='btn btn-sm' title='Añadir Proyectos' style='text-align:center'>" +
                          "<font color='#3d8bba' size='2'><span aria-hidden='true' class='fa fa-briefcase'></span></font>" +
                      "</a>"+*/
                    /*
                    
                    onclick="resultado('("AgregarClientes_Permisos", "Mantenimiento", new { id = "param-id" })','@IdUsuario','@Url.Action("Eliminar_MantRolUsuario","Mantenimiento", new {id = "param-id"})')">Crear</button>
                                            */
/*...
                    "<button type='button' id='" + item.Id_Cliente + "' name='" + item.Nombre_Cliente + "' class='btn btn-sm class_Agregar_ManPermiso_Contratos' title='Agregar Contrato' style='background:none;' data-toggle='modal' data-target='#modalAgregarContratos' data-id='" + item.Id_Cliente + "' onclick= 'mostrarDivContrato(this)'  >" +//data-url='UrlAction("ClearData", "Home", new { id ="param-name" })'>"+
                    "<font color='#0d860d' size='2'><span aria-hidden='true' class='fa fa-book'></span></font>" +
                    "</button>" +....*/

                    /*"<button id='btnContrato' type='button' class='btn btn-sm' value='' onclick='arDivContrato(this)' style='background:none'>"+
                            "<font color='#0d860d' size='2'><span aria-hidden='true' class='fa fa-book'></span></font>" +
                        "</button>"+// &nbsp;"+*/

                  /*.....  "<button id='btnProyecto' type='button' class='btn btn-sm' value='' onclick='mostrarDivProyecto()' style='background:none'>" +
                    "<font color='#3d8bba' size='2'><span aria-hidden='true' class='fa fa-briefcase'></span></font>" +
                    "</button>" +
                    "</td>" +
                    "<td id='" + item.Id_Cliente + "TD'>" + item.Nombre_Cliente ...*/
                    "<td id='" + item.Id_Contrato + "TD'>" + item.Nombre + "</td>" +
                    "<td>" + NombreCliente+"</td></tr>");
            });


            /* if (mensaje.indexOf('!') != -1) {
                 $("#DivDanger").css('display', 'block');
                 $("#CadenaDanger").html(mensaje);
                 $("#DivDanger").delay(4000).fadeOut(300);
                 console.log(mensaje); //alert(mensaje);
             } else {
                 $("#DivSucess").css('display', 'block');
                 $("#CadenaSucess").html(mensaje);
                 $("#DivSucess").delay(2000).fadeOut(300);
                 console.log(mensaje); //alert(mensaje);
                 location.reload();
             }*/

            //$('#btnSalir').trigger('click');
            document.getElementById('btnSalirContrato').click();
            // $('#modalAgregarCliente').modal("dispose");


        },
        error: function () {
            //$("#DivError").css('display', 'block');
            //$("#DivError").delay(1000).fadeOut(300);
        }


    });
}
function EliminarFilaContrato(x1) {
    //  alert(x.id);

    var IDBoton = x1.id;
    var Separador = IDBoton.split("_");
    var Id = Separador[1];
    $('.' + Id + 'TR').remove();
    //console.log(Id+'TR');
}

/*-----------------------------------------------------------------------
    JavaScript de Agregar Proyectos de acuerdo al clientes seleccionado
  -----------------------------------------------------------------------*/

function doSearch() {
    var tableReg = document.getElementById('tablaProyectos');
    var searchText = document.getElementById('searchTxtProyecto').value.toLowerCase();
    var cellsOfRow = "";
    var found = false;
    var compareWith = "";

    // Recorremos todas las filas con contenido de la tabla
    for (var i = 1; i < tableReg.rows.length; i++) {
        cellsOfRow = tableReg.rows[i].getElementsByTagName('td');
        found = false;
        // Recorremos todas las celdas
        for (var j = 0; j < cellsOfRow.length && !found; j++) {
            compareWith = cellsOfRow[j].innerHTML.toLowerCase();
            // Buscamos el texto en el contenido de la celda
            if (searchText.length == 0 || (compareWith.indexOf(searchText) > -1)) {
                found = true;
            }
        }
        if (found) {
            tableReg.rows[i].style.display = '';
        } else {
            // si no ha encontrado ninguna coincidencia, esconde la
            // fila de la tabla
            tableReg.rows[i].style.display = 'none';
        }
    }
}




/*
$(function () {
    $("#searchTxtProyecto").keyup(function () {
        var texto = $(this).val();

        $("#tablaProyectos tr").each(function () {
            var resultadoTabla = $(this).text().toLowerCase().indexOf(texto.toLowerCase());

            if (resultadoTabla < 0) {
                $(this).fadeOut();
            }
            else {
                $(this).fadeIn();
            }
        });
    });
});*/
// Cargar Proyectos a Modal 
function mostrarDivProyecto(btnproy) {//url1) {

    var url = urlAddProyectos;//UrlAction'AddContratosClientes_Modal','Mantenimiento', new { id ='param-id'});//@Url.Action("AddContratosClientes_Modal","Mantenimiento", new { id ="param-name"});

    var nameBotonClick = $('#NombreClienteProyecto').text(btnproy.name);
    /*
    var UrlM = location.href;//CadUrl+"";
    var datearray1 = UrlM.split("/");
    var Usuario = datearray1[datearray1.length - 1];*/

    //alert('proyecto' + btn.id);
    //alert('proyecto url:' + url);
    url = url.replace("param-id", btnproy.id);
    AjaxLlenarProyectos("tbodyProyectos", url, btnproy.id);
   
    
}

var VecesPaginacionProy = 0;
function AjaxLlenarProyectos(idproy, url,IdClienteBoton) {


    var cont = 0;
    //alert(id);
    $.ajax({
        type: 'GET',
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#" + idproy).empty();
          //  console.log('ProyectoGral:');
          //  console.log(data);

            $.each(data, function (i, item) {


                //var itemRepetido = validaProyIngresado(odigoInterno);
                //if (itemRepetido == 0) {
                    $("#" + idproy).append("<tr>"
                        + "<td nowrap> <input type='checkbox' name='" + IdClienteBoton + "' value='" + data[i].Codigo_Interno + "' id='" + data[i].Id_Proyecto + "' class='checkproyectos'/>"+data[i].Codigo_Interno+"</td></tr>");
                    cont++;
                //} else {}

                
            });
            /* *********
            if (VecesPaginacionProy > 0) {
                $('#nav').remove();
            }

            $('#tablaProyectos').after('<div id="nav"></div>');

            var rowsShown = 4;//10;
            var rowsTotal = $('#tablaProyectos tbody tr').length;
            var numPages = rowsTotal / rowsShown;
            for (i = 0; i < numPages; i++) {
                var pageNum = i + 1;
                //....Paginacion
                $('#nav').append('<a href="#" rel="' + i + '">' + pageNum + '</a> ');
                VecesPaginacionProy++;
            }
            $('#tablaProyectos tbody tr').hide();
            $('#tablaProyectos tbody tr').slice(0, rowsShown).show();
            $('#nav a:first').addClass('active');
            $('#nav a').bind('click', function () {

                $('#nav a').removeClass('active');
                $(this).addClass('active');
                var currPage = $(this).attr('rel');
                var startItem = currPage * rowsShown;
                var endItem = startItem + rowsShown;
                $('#tablaProyectos tbody tr').css('opacity', '0.0').hide().slice(startItem, endItem).
                    css('display', 'table-row').animate({ opacity: 1 }, 300);
            });**** */
          },
        error: function () {
            //$("#DivError").css('display', 'block');
            //$("#DivError").delay(1000).fadeOut(300);
        }


    });
}


//agregar a la tabla de Proyecto
function AgregarTablaProyecto(btnProye) {
    var urlp = urlAddProyectoTabla;
    var NombreClienteProy = $('#NombreClienteProyecto').text();
   // alert(NombreCliente);
    var n = 0;
    var resultadoProyecto = [];
    var idClienteCheckProy;

    // ***var bandRepetidos = 0;
    $('.checkproyectos:checked').each(function () {
        var Id_Proyecto = $(this).prop("id");
        var Codigo_Interno = $(this).val();//$(this).prop("value");
        idClienteCheckProy = $(this).prop("name");
       
    //***    console.log("IDProyecto:" + Id_Proyecto + " CodInterno:" + Codigo_Interno);
        //console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") Seleccionado");

        var resp = validaProyIngresado(Codigo_Interno);
       // alert('Capturado:' + resp);

       //if (resp ==0) {
            var fila = {
                Id_Proyecto,
                Codigo_Interno
            };
            resultadoProyecto.push(fila); n++;
       // } else if (resp == 1){
        //}

         if (resp> 0) {
             
             $('#'+resp).remove();
         }
        
    });
    if (n > 0) {
        // var url = url1;//'UrlAct"AgregarClientes_Permisos", "Mantenimiento", new { id = "param-id" });'
        ///url = url.replace("param-id", Usuario);

        //contAjaxTabla++;
        // alert(url);
        ejecutaAjaxtablaProyecto("tbodyProyectosAdd", NombreClienteProy, resultadoProyecto, urlp, idClienteCheckProy);
    } else {
        swal('', 'Para agregar un Proyecto debe seleccionar un checkbok', 'error');
    }

}





function TodoContratos(btnC) {
    var filasCont = $("#tbodyContratosAdd").find("tr");
    CadContratos = '<div><center><table class="" style="font-size=12px;" >';
    for (var e = 0; e < filasCont.length; e++) {

        var celdasCont = $(filasCont[e]).find("td");
        IdContratos = $(celdasCont[3]).text();

        CadContratos = CadContratos + '<tr><td>' + IdContratos + '</td></tr>' + '<tr style="height:10px"></tr>';

    }
    CadContratos = CadContratos + '</table></center></div>';

    //    console.log(CadProyectos);

    swal({
        title: '<h5><strong><label style="color:#3085d6">Contratos a agregar</label></strong></h5>',
        // type: 'info',
        html: CadContratos,
        focusConfirm: false,
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Ok',
        confirmButtonAriaLabel: 'Thumbs',
    });

}

//ver todos
function TodosProyectos(btnP) {
    var filasProyectos = $("#tbodyProyectosAdd").find("tr");
    CadProyectos = '<div><center><table class="" style="font-size=12px;" >';
    for (var e = 0; e < filasProyectos.length; e++) {

        var celdasProy = $(filasProyectos[e]).find("td");
        IdEmpresaProyT = $(celdasProy[3]).text();

        CadProyectos = CadProyectos + '<tr><td>' + IdEmpresaProyT + '</td></tr>' +'<tr style="height:10px"></tr>';
        
    }
    CadProyectos = CadProyectos+'</table></center></div>';

//    console.log(CadProyectos);

    swal({
        title: '<h5><strong><label style="color:#3085d6">Proyectos a agregar</label></strong></h5>',
        // type: 'info',
        html: CadProyectos,
        focusConfirm: false,
        confirmButtonText:'<i class="fa fa-thumbs-up"></i> Ok',
        confirmButtonAriaLabel: 'Thumbs',
    });

}

function ejecutaAjaxtablaProyecto(id, NombreClienteProy, resultadoProyecto, urlp, idClienteCheckProy) {

    //   var Actioncontrato = @Url.Action("AddContratosClientes_Modal","Mantenimiento", new { id ="param-name"});//'@Url.Action("AddContratosClientes_Modal","Mantenimiento", new { id ="param-name"})';



    var jsonObject = { lkItems: resultadoProyecto }// Usuario: Usuario }

    $.ajax({
        type: 'POST',
        url: urlp,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonObject),

        success: function (data) {
            //..........    $("#" + id).empty();onclick='ResaltaFila(this)'
              console.log(data);
            $.each(data, function (i, item) {
                $("#" + id).append("<tr  id='" + item.Id_Proyecto + "' class='" + item.Id_Proyecto + "TR' name='" + idClienteCheckProy + "' >" +
                    "<td style='display:none;'>" + idClienteCheckProy + "</td>"+
                    "<td style='display:none;'>" + item.Id_Proyecto + "</td>"+                   
                    "<td nowrap>" +
                        "<button id='btnEliminar_" + item.Id_Proyecto + "' type='button' class='btn btn-sm' value='' onclick='EliminarFilaProy(this)' style='background:none'>" +
                        "<font color='#d31519' size='2'><span aria-hidden='true' class='fa fa-trash-o'></span></font>" +
                        "</button>" +// &nbsp;"+ 
                    "</td>" +
                    
                    "<td id='" + item.Id_Proyecto + "TD'>" + item.Codigo_Interno + "</td>" +
                    "<td>" + NombreClienteProy + "</td></tr>");
            });

            


            /* if (mensaje.indexOf('!') != -1) {
                 $("#DivDanger").css('display', 'block');
                 $("#CadenaDanger").html(mensaje);
                 $("#DivDanger").delay(4000).fadeOut(300);
                 console.log(mensaje); //alert(mensaje);
             } else {
                 $("#DivSucess").css('display', 'block');
                 $("#CadenaSucess").html(mensaje);
                 $("#DivSucess").delay(2000).fadeOut(300);
                 console.log(mensaje); //alert(mensaje);
                 location.reload();
             }*/

            //$('#btnSalir').trigger('click');
            document.getElementById('btnSalirProyecto').click();
            // $('#modalAgregarCliente').modal("dispose");


        },
        error: function () {
            //$("#DivError").css('display', 'block');
            //$("#DivError").delay(1000).fadeOut(300);
        }
        
    });
}
function EliminarFilaProy(x2) {
    //  alert(x.id);

    var IDBoton = x2.id;
    var Separador = IDBoton.split("_");
    var Id = Separador[1];
    $('.' + Id + 'TR').remove();
    //console.log(Id+'TR');
}




function validaProyIngresado(itemProyect) {

    var filasProyectos = $("#tbodyProyectosAdd").find("tr");
    var resultadoProyectos = [];// = "";
    for (var e = 0; e < filasProyectos.length; e++) {
        
    var celdasProy = $(filasProyectos[e]).find("td");
    IdEmpresaProyT = $(celdasProy[3]).text();
   // alert('Check:' + itemProyect + ' tabla:' + IdEmpresaProyT + ' n:' + filasProyectos.length);
    if (itemProyect == IdEmpresaProyT) {
          // alert('Ya ingresado');
          //  swal('', 'Favor verificar, El proyecto ya se encuentra ingresado.', 'error');
        return ($(celdasProy[1]).text());
            
        } else {           // alert('No existe:' + IdEmpresaProyT);
            return (0);
           }
    } //alert('0 ... . . .');
    return 0;//return por defecto si esta vacio
    
}

//****************************Boton Guardar*************************************

function GuardarRegistro(btnGuardar) {
    
    var resultadoCheck = [];
    //$('input[type=checkbox]:checked').each(function () {
    $('.checkOpciones:checked').each(function () {
        var IdCheck = $(this).prop("id");
        var Valor = 'S';// $(this).val();//$(this).prop("value");
      //  var fila = {
        //    IdCheck,
          //  Valor
        //};
       //"IdCheck:" + IdCheck + " Valor:" + Valor);
        resultadoCheck.push(IdCheck);
        contOpcion++;
       // n++;
    });
 console.log(resultadoCheck);


    /* NO HASTA SEGUNDO DEMO
    var filas = $("#tbodyClientes").find("tr"); //devulve las filas del body de tu tabla
    var resultado = [];// = "";
    var r = "";

    for (i = 0; i < filas.length; i++) { //Recorre las filas 1 a 1
            var celdas = $(filas[i]).find("td"); //devolverá las celdas de una fila
            IdEmpresa = $(celdas[0]).text();
            IdEmpresa = IdEmpresa.replace(" ", "");
            //var idFila = ($(filas[i])).attr('id');
            console.log(IdEmpresa + ':');
            //alert('idFila:' + idFila + '... ' + celdas.id + ' clas:' + celdas.attr('id'));
    }
    */

    //Recorrer tabla Contratos
    var filasContratos = $("#tbodyContratosAdd").find("tr"); //devulve las filas del body de tu tabla
    //....var resultadoContrato = [];// = "";

    var ArrayMTPermiso = [];
    for (i = 0; i < filasContratos.length; i++) { //Recorre las filas 1 a 1

       var celdas = $(filasContratos[i]).find("td"); //devolverá las celdas de una fila
       IdEmpresaContT = $(celdas[0]).text();
       IdEmpresaContT = IdEmpresaContT.replace(" ", "");

       IdContratoT = $(celdas[1]).text();
       IdContratoT = IdContratoT.replace(" ", "");

       Id_Usuario = IdUser;
       Id_Cliente = IdEmpresaContT;
       Id_Contrato = IdContratoT;

       
       Id_Proyecto = null;
       Consultar = 'N';
       Modificar = 'N';
       Crear = 'N';
       Eliminar = 'N';
       Aprobar = 'N';

       if (resultadoCheck.includes("checkConsultar")) {
           Consultar = 'S';
       }
       if (resultadoCheck.includes("checkModificar")) {
           Modificar = 'S';
       }
       if (resultadoCheck.includes("checkCrear")) {
           Crear = 'S';
       }
       if (resultadoCheck.includes("checkEliminar")) {
           Eliminar = 'S';
       }
       if (resultadoCheck.includes("checkAprobar")) {
           Aprobar = 'S';
       }
      /*
       Usuario = ;
       Fecha_Registro = ;
       Estado = 'A';       */
       Usuario = null;
       Fecha_Registro = null;
       Estado = null;

       var MTPermiso = {

           //Id_Permiso,
           Id_Usuario,
           Id_Cliente,
           Id_Contrato,
           Id_Proyecto,
           //Id_Tipo_Trabajo,
           //Id_Linea,
           Consultar,
           Modificar,
           Crear,
           Eliminar,
           Aprobar,
           Usuario,
           Fecha_Registro,
           Estado
       }
       ArrayMTPermiso.push(MTPermiso);
       contTabla++;
      // console.log(ArrayMTPermiso);
       //console.log('IdEmpresaCont:' + IdEmpresaContT + '  IdContratoT:'+IdContratoT);
    }

    var filasProyectos = $("#tbodyProyectosAdd").find("tr");
    var resultadoProyectos = [];// = "";
    for (vs = 0; vs < filasProyectos.length; vs++) { 

        var celdasProy = $(filasProyectos[vs]).find("td"); //devolverá las celdas de una fila
        IdEmpresaProyT = $(celdasProy[0]).text();
        IdEmpresaProyT = IdEmpresaProyT.replace(" ", "");

        IdProyectoT = $(celdasProy[1]).text();
        IdProyectoT = IdProyectoT.replace(" ", "");

        Id_Usuario = IdUser;
        Id_Cliente = IdEmpresaProyT;
        Id_Contrato= null;
        Id_Proyecto = IdProyectoT;

        Consultar = 'N';
        Modificar = 'N';
        Crear = 'N';
        Eliminar = 'N';
        Aprobar = 'N';

        Usuario = null;
        Fecha_Registro = null;
        Estado = null;

        if (resultadoCheck.includes("checkConsultar")) {
            Consultar = 'S';
        }
        if (resultadoCheck.includes("checkModificar")) {
            Modificar = 'S';
        }
        if (resultadoCheck.includes("checkCrear")) {
            Crear = 'S';
        }
        if (resultadoCheck.includes("checkEliminar")) {
            Eliminar = 'S';
        }
        if (resultadoCheck.includes("checkAprobar")) {
            Aprobar = 'S';
        }


        // 
        /*
         Usuario = ;
         Fecha_Registro = ;
         Estado = 'A';       */

        var MTPermisoProy = {

            //Id_Permiso,
            Id_Usuario,
            Id_Cliente,
            Id_Contrato,
            Id_Proyecto,
            //Id_Tipo_Trabajo,
            //Id_Linea,
            Consultar,
            Modificar,
            Crear,
            Eliminar,
            Aprobar,
            Usuario,
            Fecha_Registro,
            Estado
        }
        ArrayMTPermiso.push(MTPermisoProy);
        contTabla++;
        
       // console.log('IdEmpresaProyT:' + IdEmpresaProyT + '  IdProyectoT:' + IdProyectoT);
    }
 console.log(ArrayMTPermiso);
    //var UrlPermiso = location.href;//CadUrl+"";
    //var datearray1 = UrlPermiso.split("/");
 //var IdUser = datearray1[datearray1.length - 1];
 var urlG = urlGuardar;
 AjaxGuardarMT_Permisos(urlG, ArrayMTPermiso);
}

function AjaxGuardarMT_Permisos(urlG, ArrayMTPermiso) {
    var jsonObjectPer = { lkItemsPermisos: ArrayMTPermiso};
    if (contTabla > 0) {
        if (contOpcion >0) {
              $.ajax({
                    type: 'POST',
                    url: urlG,
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(jsonObjectPer),

                    success: function (mensaje) {
                        if (mensaje.indexOf('!') != -1) {
                            $("#DivDanger").css('display', 'block');
                            $("#CadenaDanger").html(mensaje);
                            $("#DivDanger").delay(4000).fadeOut(300);
                            console.log(mensaje); //alert(mensaje);
                        } else {
                            $("#DivSucess").css('display', 'block');
                            $("#CadenaSucess").html(mensaje);
                            $("#DivSucess").delay(2000).fadeOut(300);
                            console.log(mensaje); //alert(mensaje);
                            //location.reload();

                            var url = urlPrincipal;//'Url.Action("MantPermisos", "Mantenimiento")';
                            window.location.href = url;
                        }
                    },
                    error: function () {
                        //$("#DivError").css('display', 'block');
                        //$("#DivError").delay(1000).fadeOut(300);
                    }

                });
        } else {
            swal('', 'Favor seleccionar un acción antes de guardar.', 'error');
        }
    }/* else { swal('', 'Favor agregar proyectos o contratos antes de guardar.', 'error'); } oilas comente esto para q deje guardar ciente*/
    //var jsonObject = { lkItems: ArrayMTPermiso};

  
}
/*
$(".alert").fadeTo(2000, 500).slideUp(500, function () {
    $(".alert").slideUp(500);
});*/
var contGlobal = 0;
function contarClients() {
   
    var filasCli = $("#tbodyClientes").find("tr");
    var contarclientes = 0;
    var nnn = 0;
    for (qb = 0; qb < filasCli.length; qb++) { 
          //  var celdas = $(filas[qb]).find("td"); 
        contarclientes++;
    }
    if (contarclientes==0) {
        $("#tbodyClientes").empty();
        $("#tbodyContratosAdd").empty();
        $("#tbodyProyectosAdd").empty();
      
        swal('', 'No existen clientes asignados.', 'info');
        contGlobal = 0;
    }
    if (contGlobal >0) {
        if (contarclientes==0 ) {
        location.reload();
       }
    }
  contGlobal++;
}


//Paginacion JQuery
/*$('#tablaProyectos').DataTable({
    "pagingType": "full_numbers"
});*/