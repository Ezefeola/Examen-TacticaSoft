@ModelType Examen.BL.ClienteModel

@Code
    ViewData("Title") = "Editar Producto"
End Code

<h2>Editar Producto</h2>

@Using (Html.BeginForm("Update", "Clientes", New With {.ID = Model.ID}, FormMethod.Post))
    @Html.AntiForgeryToken()

    @<div Class="form-horizontal">

    <h4>Producto</h4>
    <hr />
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

    @Html.HiddenFor(Function(model) model.ID)

    <div class="form-group">
        @Html.LabelFor(Function(model) model.Cliente, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.EditorFor(Function(model) model.Cliente, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.Cliente, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.Telefono, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.EditorFor(Function(model) model.Telefono, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.Telefono, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.Correo, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.EditorFor(Function(model) model.Correo, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.Correo, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group mt-3">
        <div class="col-md-offset-2 col-md-10">
            <input type="submit" value="Guardar" class="btn btn-primary" />
        </div>
    </div>
</div>

End Using

<div class="mt-3">
    @Html.ActionLink("Volver a la Lista", "Index")
</div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
