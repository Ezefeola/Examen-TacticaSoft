Imports System.Web.Mvc
Imports Examen.BL
Imports Examen.DAL

Namespace Controllers
    Public Class ProductosController
        Inherits Controller

        Private ReadOnly _productoRepository As IProductoRepository
        Public Sub New(productoRepository As IProductoRepository)
            _productoRepository = productoRepository
        End Sub

        ' GET: Client
        Function Index(searchTerm As String, filterOption As String) As ActionResult
            Dim productos As List(Of ProductoModel)

            ' Filtrar clientes según el término de búsqueda y opción de filtro
            If String.IsNullOrEmpty(searchTerm) Then
                productos = _productoRepository.GetAll()
            Else
                Select Case filterOption
                    Case "IDProducto"
                        productos = _productoRepository.SearchByNombre(searchTerm)
                    Case "Precio"
                        productos = _productoRepository.SearchByPrecio(searchTerm)
                    Case "Categoria"
                        productos = _productoRepository.SearchByCategoria(searchTerm)
                    Case Else
                        productos = _productoRepository.GetAll()
                End Select
            End If
            ViewBag.FilterOptions = New SelectList({"Nombre", "Precio", "Categoria"})
            Return View(productos)
        End Function

        <HttpGet>
        Public Function GetPrecioProducto(id As Integer) As JsonResult
            Dim producto = _productoRepository.GetProductoById(id)
            If producto IsNot Nothing Then
                Return Json(New With {.precio = producto.Precio}, JsonRequestBehavior.AllowGet)
            End If
            Return Json(Nothing, JsonRequestBehavior.AllowGet)
        End Function

        Function Search(searchTerm As String, filterOption As String) As JsonResult
            Dim productos As List(Of ProductoModel)
            If String.IsNullOrEmpty(searchTerm) Then
                productos = _productoRepository.GetAll()
            Else
                'clientes = _clienteRepository.SearchClientes(searchTerm)
                Select Case filterOption
                    Case "Nombre"
                        productos = _productoRepository.SearchByNombre(searchTerm)
                    Case "Precio"
                        productos = _productoRepository.SearchByPrecio(searchTerm)
                    Case "Categoria"
                        productos = _productoRepository.SearchByCategoria(searchTerm)
                    Case Else
                        productos = _productoRepository.GetAll()
                End Select
            End If
            Return Json(productos, JsonRequestBehavior.AllowGet)
        End Function

        Function Create() As ActionResult
            Return View()
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Create(nombre As String, precio As String, categoria As String) As ActionResult

            If ModelState.IsValid Then

                Dim nuevoProducto As New ProductoModel With {
                    .Nombre = nombre,
                    .Precio = precio,
                    .Categoria = categoria
                }

                _productoRepository.Add(nuevoProducto)

                Return RedirectToAction("Index")
            End If

            Return View("Create")
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Delete(ID As Integer) As ActionResult

            Try

                _productoRepository.Delete(ID)
                Return RedirectToAction("Index")
            Catch ex As Exception

            End Try


            Return View("Index")

        End Function


        Function Update(ID As Integer) As ActionResult
            Dim producto As ProductoModel = _productoRepository.GetProductoById(ID)

            If producto Is Nothing Then
                Return HttpNotFound()
            End If
            Return View(producto)
        End Function


        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Update(id As Integer, nombre As String, precio As String, categoria As String) As ActionResult

            If ModelState.IsValid Then

                Dim productoActualizado As New ProductoModel With {
                    .ID = id,
                    .Nombre = nombre,
                    .Precio = precio,
                    .Categoria = categoria
                }

                _productoRepository.Update(productoActualizado)

                Return RedirectToAction("Index")
            End If

            Return View("Update")
        End Function



    End Class
End Namespace