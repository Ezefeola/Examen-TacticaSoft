@ModelType Examen.BL.VentaModel



<div class="container">
    <h2>Detalle de Venta</h2>
    <hr />
    <div class="d-flex flex-wrap gap-5 justify-content-around">
        <div>
            <h3>Cliente</h3>
            <dl class="dl-horizontal">
                <dt>ID:</dt>
                <dd>@Model.ID</dd>

                <dt>Cliente:</dt>
                <dd>@ViewBag.Cliente.Cliente</dd> @* Asumiendo que el cliente está relacionado en tu modelo *@

                <dt>Telefono:</dt>
                <dd>@ViewBag.Cliente.Telefono</dd>

                <dt>Correo:</dt>
                <dd>@ViewBag.Cliente.Correo</dd>
            </dl>
        </div>
        <div>
            <h3>Venta</h3>
            <dl class="dl-horizontal">
                <dt>ID:</dt>
                <dd>@Model.ID</dd>

                <dt>Cliente:</dt>
                <dd>@ViewBag.Cliente.Cliente</dd> @* Asumiendo que el cliente está relacionado en tu modelo *@

                <dt>Fecha:</dt>
                <dd>@Model.Fecha.ToString("dd/MM/yyyy")</dd>

                <dt>Total:</dt>
                <dd>@Model.Total.ToString("C")</dd>
            </dl>
        </div>
        <div>
            <h3>Producto</h3>

            <dl class="dl-horizontal">
                <dt>Nombre:</dt>
                <dd>@ViewBag.Producto.Nombre</dd>
                <dt>Categoría:</dt>
                <dd>@ViewBag.Producto.Categoria</dd>

                <dt>Precio:</dt>
                <dd>@ViewBag.Producto.Precio</dd> @* Asumiendo que el cliente está relacionado en tu modelo *@
                <dt>Cantidad:</dt>
                <dd>@Model.Items.Cantidad</dd>

            </dl>
        </div>
    </div>
    <div class="form-group">
        @Html.ActionLink("Volver a la Lista", "Index", Nothing, New With {.class = "btn btn-default"})
    </div>
</div>
