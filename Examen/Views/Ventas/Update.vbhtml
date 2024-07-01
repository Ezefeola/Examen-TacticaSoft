@ModelType Examen.BL.VentaModel

@Using (Html.BeginForm("Update", "Ventas", New With {.ID = Model.ID}, FormMethod.Post))
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
    <h4>Venta</h4>
    <hr />
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

    @Html.HiddenFor(Function(model) model.ID)
    @Html.HiddenFor(Function(model) model.Total)
    @Html.HiddenFor(Function(model) model.Fecha)

    <div class="form-group">
        <div class="col-md-10">
            <label class="control-label col-md-2" for="IDCliente">Cliente</label>
            @Html.HiddenFor(Function(model) model.IDCliente)
            @Html.DropDownListFor(Function(model) model.IDCliente, New SelectList(ViewBag.Clientes, "Value", "Text", Model.IDCliente), "Seleccione un cliente...", New With {.disabled = "disabled", .class = "form-control"})
            @Html.ValidationMessageFor(Function(model) model.IDCliente, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.Fecha, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.EditorFor(Function(model) model.Fecha, New With {.htmlAttributes = New With {.disabled = "disabled", .class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.Fecha, "", New With {.class = "text-danger"})
        </div>
    </div>

    
    <div id="ventaItemsContainer">
        <div class="form-group">
            <label for="IDProducto">Producto</label>
            @Html.DropDownList("IDProducto", New SelectList(ViewBag.Productos, "Value", "Text", ViewBag.Venta.Items.IDProducto), "Seleccione un producto...", New With {.class = "form-control producto-select"})
        </div>
        <div class="form-group">
            <label for="PrecioUnitario">Precio Unitario</label>
            <input type="number" step="0.01" name="PrecioUnitario" class="form-control precio-unitario" value="@ViewBag.Venta.Items.PrecioUnitario" readonly />
        </div>
        <div class="form-group">
            <label for="Cantidad">Cantidad</label>
            <input type="number" name="Cantidad" class="form-control cantidad" value="@ViewBag.Venta.Items.Cantidad" />
        </div>
        <div class="form-group">
            <label for="PrecioTotal">Precio Total</label>
            <input type="number" step="0.01" name="PrecioTotal" class="form-control precio-total" value="@ViewBag.Venta.Items.PrecioTotal" readonly />
        </div>


    </div>

    <div class="form-group mt-3">
        <div class="col-md-offset-2 col-md-10">
            <input type="submit" value="Guardar" class="btn btn-primary" />
        </div>
    </div>
    <div class="mt-2">
        @Html.ActionLink("Volver al listado", "Index")
    </div>
</div>
End Using

@section Scripts
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            $('.producto-select').change(function () {
                var productoId = $(this).val();
                var precioUnitarioInput = $('.precio-unitario')
                console.log()


                if (productoId) {
                    $.ajax({
                        url: '@Url.Action("GetPrecioProducto", "Productos")',
                        type: 'GET',
                        data: { id: productoId },
                        success: function (data) {
                            precioUnitarioInput.val(data.precio);
                            calcularPrecioTotal();

                        }
                    });
                }
            });

            $('.cantidad').keyup(function () {
                calcularPrecioTotal();
            });

            function calcularPrecioTotal() {
                $('.producto-select').each(function (index) {
                    var precioUnitario = parseFloat($('[name="PrecioUnitario"]').val());
                    var cantidad = parseFloat($('[name="Cantidad"]').val());
                    var precioTotal = precioUnitario * cantidad;
                    $('[name="PrecioTotal"]').val(precioTotal.toFixed(2));
                });

                var totalGeneral = 0;
                $('[name*="PrecioTotal"]').each(function () {
                    totalGeneral += parseFloat($(this).val());
                });
                $('[name="Total"]').val(totalGeneral.toFixed(2));
            }
        });
    </script>
End Section
