@ModelType Examen.BL.VentaModel
@Imports Newtonsoft.Json

@Code
    ViewData("Title") = "Crear Venta"
End Code

@Using Html.BeginForm("Create", "Ventas", FormMethod.Post)
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
    <h4> Venta</h4>
    <hr />
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
    @Html.HiddenFor(Function(model) model.ID)
    @Html.HiddenFor(Function(model) model.Total)

    <div class="form-group">
        @Html.LabelFor(Function(model) model.IDCliente, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.DropDownList("IDCliente", DirectCast(ViewBag.Clientes, SelectList), "Seleccione un cliente...", New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(model) model.IDCliente, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div id="ventaItemsContainer">
        <!-- Aca se agregan dinamicamente los ítems de venta -->
    </div>


    <div class="d-flex gap-3 mt-3">
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Crear" class="btn btn-primary" />
            </div>
        </div>
        <div class="form-group">
            <input type="button" value="Agregar Producto" id="agregarProducto" class="btn btn-secondary" />
        </div>
    </div>
    <div class="mt-2">
        @Html.ActionLink("Volver al listado", "Index")
    </div>
</div>
End Using

@section Scripts
    <script>
        $(document).ready(function () {
            var itemIndex = 0;

            $("#agregarProducto").click(function () {
                var itemHtml = `
                    <div class="form-group">
                        <label for="IDProducto">Producto</label>
                        @Html.DropDownList("IDProducto", DirectCast(ViewBag.Productos, SelectList), "Seleccione un producto...", New With {.class = "form-control producto-select"})
                    </div>
                    <div class="form-group">
                        <label for="Items_${itemIndex}__PrecioUnitario">Precio Unitario</label>
                        <input type="number" step="0.01" name="PrecioUnitario" class="form-control precio-unitario" readonly />
                    </div>
                    <div class="form-group">
                        <label for="Items_${itemIndex}__Cantidad">Cantidad</label>
                        <input type="number" name="Cantidad" class="form-control cantidad" />
                    </div>
                    <div class="form-group">
                        <label for="Items_${itemIndex}__PrecioTotal">Precio Total</label>
                        <input type="number" step="0.01" name="PrecioTotal" class="form-control precio-total" readonly />
                    </div>
                `;
                $("#ventaItemsContainer").append(itemHtml);
                document.querySelector('#agregarProducto').remove();
                itemIndex++;
                
            });

            $(document).on("change", ".producto-select", function () {
                var productoSelect = $(this);
                var precioUnitarioInput = productoSelect.closest(".form-group").next().find(".precio-unitario");

                var productoId = parseInt(productoSelect.val());
                if (productoId) {
                    $.ajax({
                        url: '@Url.Action("GetPrecioProducto", "Productos")',
                        data: { id: productoId },
                        success: function (result) {
                            if (result && result.precio) {
                                precioUnitarioInput.val(result.precio);
                                precioUnitarioInput.trigger("input");
                            }
                        }
                    });
                }
            });

            $(document).on("input", "[name*='Cantidad'], [name*='PrecioUnitario']", function () {
                var container = $(this).closest(".form-group").parent();
                var cantidad = container.find("[name*='Cantidad']").val();
                var precioUnitario = container.find("[name*='PrecioUnitario']").val();
                var precioTotal = cantidad * precioUnitario;
                container.find("[name*='PrecioTotal']").val(precioTotal.toFixed(2));
                calcularTotalGeneral();
            });

            function calcularTotalGeneral() {
                var totalGeneral = 0;
                $("[name*='PrecioTotal']").each(function () {
                    totalGeneral += parseFloat($(this).val());
                });
                $("[name='Total']").val(totalGeneral.toFixed(2));
            }
        });
    </script>
End Section
