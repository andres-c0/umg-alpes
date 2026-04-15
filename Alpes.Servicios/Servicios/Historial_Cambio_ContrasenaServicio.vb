Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Seguridad

Namespace Servicios
    Public Class Historial_Cambio_ContrasenaServicio

        Private ReadOnly _datos As Historial_Cambio_ContrasenaDatos

        Public Sub New()
            _datos = New Historial_Cambio_ContrasenaDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Historial_Cambio_Contrasena) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Historial_Cambio_Contrasena)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Historial_Cambio_Contrasena
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Historial_Cambio_Contrasena)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Historial_Cambio_Contrasena)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace