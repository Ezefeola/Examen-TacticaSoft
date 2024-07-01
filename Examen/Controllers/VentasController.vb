Imports System.Web.Mvc
Imports Autofac
Imports Examen.BL
Imports Examen.DAL


Namespace Controllers
    Public Class VentasController
        Inherits Controller

        Private ReadOnly _ventaRepository As IVentaRepository
        Private ReadOnly _clienteRepository As IClienteRepository
        Private ReadOnly _productoRepository As IProductoRepository
        Public Sub New(ventaRepository As IVentaRepository,
                       clienteRepository As IClienteRepository,
                       productoRepository As IProductoRepository)

            _ventaRepository = ventaRepository
            _clienteRepository = clienteRepository
            _productoRepository = productoRepository
        End Sub

        Function Index(searchTerm As String, filterOption As String) As ActionResult
            Dim ventas As List(Of VentaModel)

            If String.IsNullOrEmpty(searchTerm) Then
                ventas = _ventaRepository.GetAll()
            Else
                Select Case filterOption
                    Case "Cliente"
                        ventas = _ventaRepository.SearchByCliente(searchTerm)
                    Case "Fecha"
                        ventas = _ventaRepository.SearchByFecha(searchTerm)
                    Case Else
                        ventas = _ventaRepository.GetAll()
                End Select
            End If
            Dim clientes = _clienteRepository.GetAll().ToDictionary(Function(c) c.ID, Function(c) c.Cliente)
            ViewBag.Clientes = clientes

            ViewBag.FilterOptions = New SelectList({"Cliente", "Fecha"})

            Return View(ventas)
        End Function

        Function Search(searchTerm As String, filterOption As String) As JsonResult
            Dim ventas As List(Of VentaModel)
            If String.IsNullOrEmpty(searchTerm) Then
                ventas = _ventaRepository.GetAll()
            Else
                Select Case filterOption
                    Case "Cliente"
                        ventas = _ventaRepository.SearchByCliente(searchTerm)
                    Case "Fecha"
                        ventas = _ventaRepository.SearchByFecha(searchTerm)
                    Case Else
                        ventas = _ventaRepository.GetAll()
                End Select
            End If
            Return Json(ventas, JsonRequestBehavior.AllowGet)
        End Function

        Function Create() As ActionResult

            Dim clientes = _clienteRepository.GetAll()
            Dim productos = _productoRepository.GetAll()
            ViewBag.Clientes = New SelectList(clientes, "ID", "Cliente")
            ViewBag.Productos = New SelectList(productos, "ID", "Nombre")

            Return View()
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Create(IDCliente As Integer, total As Decimal, IDProducto As Integer,
                        PrecioTotal As Decimal, PrecioUnitario As Decimal,
                        Cantidad As Integer) As ActionResult
            If ModelState.IsValid Then
                Try
                    Dim nuevoItem As New VentaItemModel With {
                        .IDProducto = IDProducto,
                        .PrecioTotal = PrecioTotal,
                        .PrecioUnitario = PrecioUnitario,
                        .Cantidad = Cantidad
                    }

                    Dim nuevaVenta As New VentaModel With {
                    .IDCliente = IDCliente,
                    .Total = total,
                    .Items = New VentaItemModel
                    }
                    nuevaVenta.Items = nuevoItem
                    _ventaRepository.Add(nuevaVenta)
                    Return RedirectToAction("Index")
                Catch ex As Exception
                    ModelState.AddModelError("", "Error al crear la venta: " & ex.Message)
                End Try
            End If

            ViewBag.Clientes = New SelectList(_clienteRepository.GetAll(), "ID", "Cliente")
            ViewBag.Productos = New SelectList(_productoRepository.GetAll(), "ID", "Nombre")
            Return View()
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Delete(ID As Integer) As ActionResult

            Try

                _ventaRepository.Delete(ID)
                Return RedirectToAction("Index")
            Catch ex As Exception

            End Try


            Return View("Index")

        End Function
        Function ShowMore(ID As Integer) As ActionResult
            Dim venta As VentaModel = _ventaRepository.GetVentaById(ID)
            Dim ventaItem As VentaItemModel = _ventaRepository.GetVentaItemById(ID)

            venta.Items = ventaItem

            ViewBag.Cliente = _clienteRepository.GetClienteById(venta.IDCliente)
            ViewBag.Producto = _productoRepository.GetProductoById(ventaItem.IDProducto)
            Return View(venta)
        End Function
        Function Update(ID As Integer) As ActionResult
            Dim venta As VentaModel = _ventaRepository.GetVentaById(ID)

            If venta Is Nothing Then
                Return HttpNotFound()
            End If

            Dim clientes = _clienteRepository.GetAll().Select(Function(p) New SelectListItem With {
                .Value = p.ID.ToString(),
                .Text = p.Cliente
            }).ToList()
            ViewBag.Clientes = clientes

            ViewBag.Productos = _productoRepository.GetAll().Select(Function(p) New SelectListItem With {
                .Value = p.ID.ToString(),
                .Text = p.Nombre
            }).ToList()
            ViewBag.Venta = venta

            Return View(venta)
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Update(ID As Integer, Fecha As DateTime, IDCliente As Integer, Total As Decimal, IDProducto As Integer,
                        PrecioTotal As Decimal, PrecioUnitario As Decimal,
            Cantidad As Integer) As ActionResult
            Dim ventaItem As New VentaItemModel() With {
                        .Cantidad = Cantidad,
                        .IDProducto = IDProducto,
                        .IDVenta = ID,
                        .PrecioTotal = PrecioTotal,
                        .PrecioUnitario = PrecioUnitario
                    }

            Dim venta As New VentaModel() With {
                        .ID = ID,
                        .IDCliente = IDCliente,
                        .Items = ventaItem,
                        .Total = Total
                        }
            If ModelState.IsValid Then
                Try
                    _ventaRepository.Update(venta)
                    Return RedirectToAction("Index")
                Catch ex As Exception
                    ModelState.AddModelError("", "Error al actualizar la venta: " & ex.Message)
                End Try
            End If

            Dim clientes = _clienteRepository.GetAll().Select(Function(p) New SelectListItem With {
                .Value = p.ID.ToString(),
                .Text = p.Cliente
            }).ToList()
            ViewBag.Clientes = clientes
            ViewBag.Productos = _productoRepository.GetAll().Select(Function(p) New SelectListItem With {
                .Value = p.ID.ToString(),
                .Text = p.Nombre
            }).ToList()
            ViewBag.Venta = venta

            Return View(venta)
        End Function

    End Class

End Namespace