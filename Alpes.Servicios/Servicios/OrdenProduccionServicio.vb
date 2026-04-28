Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class OrdenProduccionServicio

        Private ReadOnly _datos As OrdenProduccionDatos

        Public Sub New()
            _datos = New OrdenProduccionDatos()
        End Sub

        Public Function Insertar(ByVal entidad As OrdenProduccion) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As OrdenProduccion) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal ordenProduccionId As Integer) As Boolean
            Return _datos.Eliminar(ordenProduccionId)
        End Function

        Public Function ObtenerPorId(ByVal ordenProduccionId As Integer) As OrdenProduccion
            Return _datos.ObtenerPorId(ordenProduccionId)
        End Function

        Public Function Listar() As List(Of OrdenProduccion)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of OrdenProduccion)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace