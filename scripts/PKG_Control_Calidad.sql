CREATE OR REPLACE PACKAGE PKG_CONTROL_CALIDAD AS

  PROCEDURE SP_INSERTAR(
    P_ORIGEN IN Control_Calidad.origen%TYPE,
    P_ORDEN_PRODUCCION_ID IN Control_Calidad.orden_produccion_id%TYPE,
    P_RECEPCION_MATERIAL_ID IN Control_Calidad.recepcion_material_id%TYPE,
    P_RESULTADO IN Control_Calidad.resultado%TYPE,
    P_OBSERVACION IN Control_Calidad.observacion%TYPE,
    P_INSPECCION_AT IN Control_Calidad.inspeccion_at%TYPE,
    P_ESTADO IN Control_Calidad.estado%TYPE,
    P_ID OUT Control_Calidad.control_calidad_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_CONTROL_CALIDAD_ID IN Control_Calidad.control_calidad_id%TYPE,
    P_ORIGEN IN Control_Calidad.origen%TYPE,
    P_ORDEN_PRODUCCION_ID IN Control_Calidad.orden_produccion_id%TYPE,
    P_RECEPCION_MATERIAL_ID IN Control_Calidad.recepcion_material_id%TYPE,
    P_RESULTADO IN Control_Calidad.resultado%TYPE,
    P_OBSERVACION IN Control_Calidad.observacion%TYPE,
    P_INSPECCION_AT IN Control_Calidad.inspeccion_at%TYPE,
    P_ESTADO IN Control_Calidad.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Control_Calidad.control_calidad_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Control_Calidad.control_calidad_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Control_Calidad.origen%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_CONTROL_CALIDAD;
/
CREATE OR REPLACE PACKAGE BODY PKG_CONTROL_CALIDAD AS

  PROCEDURE SP_INSERTAR(
    P_ORIGEN IN Control_Calidad.origen%TYPE,
    P_ORDEN_PRODUCCION_ID IN Control_Calidad.orden_produccion_id%TYPE,
    P_RECEPCION_MATERIAL_ID IN Control_Calidad.recepcion_material_id%TYPE,
    P_RESULTADO IN Control_Calidad.resultado%TYPE,
    P_OBSERVACION IN Control_Calidad.observacion%TYPE,
    P_INSPECCION_AT IN Control_Calidad.inspeccion_at%TYPE,
    P_ESTADO IN Control_Calidad.estado%TYPE,
    P_ID OUT Control_Calidad.control_calidad_id%TYPE
  ) AS
  BEGIN
    IF P_ORDEN_PRODUCCION_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Orden_Produccion WHERE orden_produccion_id = P_ORDEN_PRODUCCION_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Orden_Produccion para orden_produccion_id');
        END IF;
      END;
    END IF;
    IF P_RECEPCION_MATERIAL_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Recepcion_Material WHERE recepcion_material_id = P_RECEPCION_MATERIAL_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Recepcion_Material para recepcion_material_id');
        END IF;
      END;
    END IF;
    INSERT INTO Control_Calidad (
      origen,
      orden_produccion_id,
      recepcion_material_id,
      resultado,
      observacion,
      inspeccion_at,
      estado,
      created_at
    ) VALUES (
      P_ORIGEN,
      P_ORDEN_PRODUCCION_ID,
      P_RECEPCION_MATERIAL_ID,
      P_RESULTADO,
      P_OBSERVACION,
      P_INSPECCION_AT,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING control_calidad_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_CONTROL_CALIDAD_ID IN Control_Calidad.control_calidad_id%TYPE,
    P_ORIGEN IN Control_Calidad.origen%TYPE,
    P_ORDEN_PRODUCCION_ID IN Control_Calidad.orden_produccion_id%TYPE,
    P_RECEPCION_MATERIAL_ID IN Control_Calidad.recepcion_material_id%TYPE,
    P_RESULTADO IN Control_Calidad.resultado%TYPE,
    P_OBSERVACION IN Control_Calidad.observacion%TYPE,
    P_INSPECCION_AT IN Control_Calidad.inspeccion_at%TYPE,
    P_ESTADO IN Control_Calidad.estado%TYPE
  ) AS
  BEGIN
    IF P_ORDEN_PRODUCCION_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Orden_Produccion WHERE orden_produccion_id = P_ORDEN_PRODUCCION_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Orden_Produccion para orden_produccion_id');
        END IF;
      END;
    END IF;
    IF P_RECEPCION_MATERIAL_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Recepcion_Material WHERE recepcion_material_id = P_RECEPCION_MATERIAL_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Recepcion_Material para recepcion_material_id');
        END IF;
      END;
    END IF;
    UPDATE Control_Calidad
       SET origen = P_ORIGEN,
         orden_produccion_id = P_ORDEN_PRODUCCION_ID,
         recepcion_material_id = P_RECEPCION_MATERIAL_ID,
         resultado = P_RESULTADO,
         observacion = P_OBSERVACION,
         inspeccion_at = P_INSPECCION_AT,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE control_calidad_id = P_CONTROL_CALIDAD_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Control_Calidad');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Control_Calidad.control_calidad_id%TYPE
  ) AS
  BEGIN
    UPDATE Control_Calidad
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE control_calidad_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Control_Calidad');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Control_Calidad.control_calidad_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT control_calidad_id,
             origen,
             orden_produccion_id,
             recepcion_material_id,
             resultado,
             observacion,
             inspeccion_at,
             created_at,
             updated_at,
             estado
        FROM Control_Calidad
       WHERE control_calidad_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT control_calidad_id,
             origen,
             orden_produccion_id,
             recepcion_material_id,
             resultado,
             observacion,
             inspeccion_at,
             created_at,
             updated_at,
             estado
        FROM Control_Calidad
       WHERE estado = 'ACTIVO'
       ORDER BY control_calidad_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Control_Calidad.origen%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT control_calidad_id,
             origen,
             orden_produccion_id,
             recepcion_material_id,
             resultado,
             observacion,
             inspeccion_at,
             created_at,
             updated_at,
             estado
        FROM Control_Calidad
       WHERE UPPER(NVL(TO_CHAR(origen), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY control_calidad_id DESC;
  END SP_BUSCAR;

END PKG_CONTROL_CALIDAD;
/
