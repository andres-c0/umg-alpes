Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Marketing

Namespace Servicios
    Public Class CuponServicio

        Private ReadOnly _datos As CuponDatos

        Public Sub New()
            _datos = New CuponDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Cupon) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Cupon)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Cupon
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Cupon)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Cupon)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace