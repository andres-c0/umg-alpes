Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class PlanProduccionServicio

        Private ReadOnly _datos As PlanProduccionDatos

        Public Sub New()
            _datos = New PlanProduccionDatos()
        End Sub

        Public Function Insertar(ByVal entidad As PlanProduccion) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As PlanProduccion) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal planProduccionId As Integer) As Boolean
            Return _datos.Eliminar(planProduccionId)
        End Function

        Public Function ObtenerPorId(ByVal planProduccionId As Integer) As PlanProduccion
            Return _datos.ObtenerPorId(planProduccionId)
        End Function

        Public Function Listar() As List(Of PlanProduccion)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of PlanProduccion)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace