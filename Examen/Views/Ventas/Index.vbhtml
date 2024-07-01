@ModelType List(Of Examen.BL.VentaModel)
@Imports NewtonSoft.Json
@Code
    ViewData("Title") = "Lista de Ventas"
End Code

<h2>Lista de Ventas</h2>

@Using Html.BeginForm("Index", "Ventas", FormMethod.Get)
    @<div class="d-flex justify-content-between mt-5">
        <div class="">
            <button class="btn btn-primary" style="height:40px">
                @Html.ActionLink("Crear venta", "Create", Nothing,
                                                        New With {.class = "text-white text-decoration-none"})
            </button>
        </div>

        <div>
            @Using Html.BeginForm("Index", "Clientes", FormMethod.Get)
                @<div class="d-flex gap-3 align-items-center">
                    <div class="d-flex gap-2">
                        <span class="text-center">Filtrar Por:</span>
                        @Html.DropDownList("filterOption", TryCast(ViewBag.FilterOptions, SelectList), Nothing, New With {.class = "form-control"})
                    </div>
                    <div>
                        Buscar: @Html.TextBox("searchTerm", Nothing, New With {.ID = "searchTerm"})
                        <input type="submit" value="Buscar" Class="btn btn-dark" />
                    </div>
                </div>
            End Using
        </div>
    </div>
End Using



<table class="table" id="ventasTable">
    <thead>
        <tr>
            <th>ID</th>
            <th>Cliente</th>
            <th>Fecha</th>
            <th>Total</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        @For Each item In Model
            @<tr>
                <td>@item.ID</td>
                <td>
                    @If ViewBag.Clientes.ContainsKey(item.IDCliente) Then
                        @ViewBag.Clientes(item.IDCliente)
                    Else
                        @item.IDCliente
                    End If
                            </td>
                <td>@item.Fecha</td>
                <td>@item.Total</td>
                <td class="d-flex align-items-center justify-content-center gap-1">
                    <button class="btn btn-primary">
                        @Html.ActionLink("Editar", "Update",
               New With {.ID = item.ID},
               New With {.class = "text-white text-decoration-none"})
                    </button>

                    <button class="btn btn-info">
                        @Html.ActionLink("Ver detalle", "ShowMore",
      New With {.ID = item.ID},
      New With {.class = "text-white text-decoration-none"})
                    </button>

                    @Using (Html.BeginForm("Delete", "Ventas", New With {.id = item.ID}, FormMethod.Post))

                        @Html.AntiForgeryToken()
                        @<button type="submit" Class="btn btn-danger">Eliminar</button>

                    End Using

                </td>
            </tr>
        Next
    </tbody>
</table>

@section Scripts {
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">
     $(document).ready(function () {
            $('#searchTerm, #filterOption').on('change keyup', function () {
                var searchTerm = $('#searchTerm').val();
                var filterOption = $('#filterOption').val();
                var clientes = @Html.Raw(JsonConvert.SerializeObject(ViewBag.Clientes));

                $.ajax({
                    url: '@Url.Action("Search", "Ventas")',
                    type: 'GET',
                    data: { searchTerm: searchTerm, filterOption: filterOption },
                    success: function (data) {
                        var tableBody = $('#ventasTable tbody');
                        tableBody.empty();
                        $.each(data, function (index, item) {
                            var clienteNombre = clientes[item.IDCliente] || item.IDCliente;
                            let formattedDate = parseDate(item.Fecha);

                            var row = '<tr>' +
                                '<td>' + item.ID + '</td>' +
                                '<td>' + clienteNombre + '</td>' +
                                '<td>' + formattedDate + '</td>' +
                                '<td>' + item.Total + '</td>' +
                                '<td class="d-flex align-items-center justify-content-center gap-1">' +
                                '<button class="btn btn-primary">' +
                                '<a href="@Url.Action("Update", "Ventas")/' + item.ID + '" class="text-white text-decoration-none">Editar</a>' +
                                '</button>' +
                                '<form action="@Url.Action("Delete", "Ventas")/' + item.ID + '" method="post">' +
                                '@Html.AntiForgeryToken()' +
                                '<button type="submit" class="btn btn-danger">Eliminar</button>' +
                                '</form>' +
                                '</td>' +
                                '</tr>';
                            tableBody.append(row);
                        });
                    }
                });
            });

         function parseDate(dateString) {
             // Extraer el número de milisegundos del formato /Date(1719421847557)/
             let timestamp = parseInt(dateString.replace("/Date(", "").replace(")/", ""));
             let date = new Date(timestamp);
             if (isNaN(date.getTime())) {  // Si no se pudo crear una fecha válida
                 return dateString;  // Devolver la cadena original si no se puede parsear
             }

             // Formatear la fecha en el formato "dd/mm/yyyy hh:mm:ss a"
             let formattedDate = `${pad2(date.getDate())}/${pad2(date.getMonth() + 1)}/${date.getFullYear()} `;
             let hours = date.getHours();
             let minutes = date.getMinutes();
             let ampm = hours >= 12 ? 'p. m.' : 'a. m.';
             hours = hours % 12;
             hours = hours ? hours : 12; // La medianoche debe ser 12, no 0
             formattedDate += `${pad2(hours)}:${pad2(minutes)}:${pad2(date.getSeconds())} ${ampm}`;

             return formattedDate;
         }

         function pad2(number) {
             return (number < 10 ? '0' : '') + number;
         }
        });
    </script>

End Section
