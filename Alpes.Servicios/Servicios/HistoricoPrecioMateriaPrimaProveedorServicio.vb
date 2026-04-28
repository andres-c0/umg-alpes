Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class HistoricoPrecioMateriaPrimaProveedorServicio

        Private ReadOnly _datos As HistoricoPrecioMateriaPrimaProveedorDatos

        Public Sub New()
            _datos = New HistoricoPrecioMateriaPrimaProveedorDatos()
        End Sub

        Public Function Insertar(ByVal entidad As HistoricoPrecioMateriaPrimaProveedor) As Boolean
            Return _datos.Insertar(entidad)
        End Function

        Public Function Eliminar(ByVal histMpProvId As Integer) As Boolean
            Return _datos.Eliminar(histMpProvId)
        End Function

        Public Function Listar() As List(Of HistoricoPrecioMateriaPrimaProveedor)
            Return _datos.Listar()
        End Function

        Public Function ListarPorProveedor(ByVal provId As Integer) As List(Of HistoricoPrecioMateriaPrimaProveedor)
            Return _datos.ListarPorProveedor(provId)
        End Function

    End Class
End Namespace