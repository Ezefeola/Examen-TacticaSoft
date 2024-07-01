Imports Examen.BL

Public Interface IProductoRepository
    Sub Add(producto As ProductoModel)
    Sub Delete(ID As Integer)
    Sub Update(producto As ProductoModel)
    Function GetAll() As List(Of ProductoModel)
    Function GetProductoById(ID As Integer) As ProductoModel
    Function SearchByNombre(searchTerm As String) As List(Of ProductoModel)
    Function SearchByPrecio(searchTerm As Decimal) As List(Of ProductoModel)
    Function SearchByCategoria(searchTerm As String) As List(Of ProductoModel)
End Interface
