@ModelType List(Of Examen.BL.ClienteModel)

@Code
    ViewData("Title") = "Lista de Clientes"
End Code

<h2>Lista de Clientes</h2>

@Using Html.BeginForm("Index", "Clientes", FormMethod.Get)
    @<div class="d-flex justify-content-between mt-5">
        <div class="">
            <button class="btn btn-primary" style="height:40px">
                @Html.ActionLink("Crear cliente", "Create", Nothing,
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



<table class="table" id="clientesTable">
    <thead>
        <tr>
            <th>ID</th>
            <th>Cliente</th>
            <th>Teléfono</th>
            <th>Correo</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        @For Each item In Model
            @<tr>
                <td>@item.ID</td>
                <td>@item.Cliente</td>
                <td>@item.Telefono</td>
                <td>@item.Correo</td>
                <td class="d-flex align-items-center justify-content-center gap-1">
                    <button class="btn btn-primary">
                        @Html.ActionLink("Editar", "Update",
                      New With {.ID = item.ID},
                      New With {.class = "text-white text-decoration-none"})
                    </button>

                    @Using (Html.BeginForm("Delete", "Clientes", New With {.id = item.ID}, FormMethod.Post))

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
                $.ajax({
                    url: '@Url.Action("Search", "Clientes")',
                    type: 'GET',
                    data: { searchTerm: searchTerm, filterOption: filterOption },
                    success: function (data) {
                        var tableBody = $('#clientesTable tbody');
                        tableBody.empty();
                        $.each(data, function (index, item) {
                            var row = '<tr>' +
                                '<td>' + item.ID + '</td>' +
                                '<td>' + item.Cliente + '</td>' +
                                '<td>' + item.Telefono + '</td>' +
                                '<td>' + item.Correo + '</td>' +
                                '<td class="d-flex align-items-center justify-content-center gap-1">' +
                                '<button class="btn btn-primary">' +
                                '<a href="@Url.Action("Update", "Clientes")/' + item.ID + '" class="text-white text-decoration-none">Editar</a>' +
                                '</button>' +
                                '<form action="@Url.Action("Delete", "Clientes")/' + item.ID + '" method="post">' +
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
        });
    </script>

End Section
