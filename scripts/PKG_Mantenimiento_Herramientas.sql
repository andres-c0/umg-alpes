CREATE OR REPLACE PACKAGE PKG_MANTENIMIENTO_HERRAMIENTAS AS

  PROCEDURE SP_INSERTAR(
    P_HERRAMIENTA_ID IN Mantenimiento_Herramienta.herramienta_id%TYPE,
    P_FECHA_MANTENIMIENTO IN Mantenimiento_Herramienta.fecha_mantenimiento%TYPE,
    P_TIPO IN Mantenimiento_Herramienta.tipo%TYPE,
    P_COSTO IN Mantenimiento_Herramienta.costo%TYPE,
    P_OBSERVACION IN Mantenimiento_Herramienta.observacion%TYPE,
    P_ESTADO IN Mantenimiento_Herramienta.estado%TYPE,
    P_ID OUT Mantenimiento_Herramienta.mantenimiento_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_MANTENIMIENTO_ID IN Mantenimiento_Herramienta.mantenimiento_id%TYPE,
    P_HERRAMIENTA_ID IN Mantenimiento_Herramienta.herramienta_id%TYPE,
    P_FECHA_MANTENIMIENTO IN Mantenimiento_Herramienta.fecha_mantenimiento%TYPE,
    P_TIPO IN Mantenimiento_Herramienta.tipo%TYPE,
    P_COSTO IN Mantenimiento_Herramienta.costo%TYPE,
    P_OBSERVACION IN Mantenimiento_Herramienta.observacion%TYPE,
    P_ESTADO IN Mantenimiento_Herramienta.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Mantenimiento_Herramienta.mantenimiento_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Mantenimiento_Herramienta.mantenimiento_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Mantenimiento_Herramienta.fecha_mantenimiento%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_MANTENIMIENTO_HERRAMIENTAS;
/
CREATE OR REPLACE PACKAGE BODY PKG_MANTENIMIENTO_HERRAMIENTAS AS

  PROCEDURE SP_INSERTAR(
    P_HERRAMIENTA_ID IN Mantenimiento_Herramienta.herramienta_id%TYPE,
    P_FECHA_MANTENIMIENTO IN Mantenimiento_Herramienta.fecha_mantenimiento%TYPE,
    P_TIPO IN Mantenimiento_Herramienta.tipo%TYPE,
    P_COSTO IN Mantenimiento_Herramienta.costo%TYPE,
    P_OBSERVACION IN Mantenimiento_Herramienta.observacion%TYPE,
    P_ESTADO IN Mantenimiento_Herramienta.estado%TYPE,
    P_ID OUT Mantenimiento_Herramienta.mantenimiento_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Herramienta WHERE herramienta_id = P_HERRAMIENTA_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Herramienta para herramienta_id');
      END IF;
    END;
    INSERT INTO Mantenimiento_Herramienta (
      herramienta_id,
      fecha_mantenimiento,
      tipo,
      costo,
      observacion,
      estado,
      created_at
    ) VALUES (
      P_HERRAMIENTA_ID,
      P_FECHA_MANTENIMIENTO,
      P_TIPO,
      P_COSTO,
      P_OBSERVACION,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING mantenimiento_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_MANTENIMIENTO_ID IN Mantenimiento_Herramienta.mantenimiento_id%TYPE,
    P_HERRAMIENTA_ID IN Mantenimiento_Herramienta.herramienta_id%TYPE,
    P_FECHA_MANTENIMIENTO IN Mantenimiento_Herramienta.fecha_mantenimiento%TYPE,
    P_TIPO IN Mantenimiento_Herramienta.tipo%TYPE,
    P_COSTO IN Mantenimiento_Herramienta.costo%TYPE,
    P_OBSERVACION IN Mantenimiento_Herramienta.observacion%TYPE,
    P_ESTADO IN Mantenimiento_Herramienta.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Herramienta WHERE herramienta_id = P_HERRAMIENTA_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Herramienta para herramienta_id');
      END IF;
    END;
    UPDATE Mantenimiento_Herramienta
       SET herramienta_id = P_HERRAMIENTA_ID,
         fecha_mantenimiento = P_FECHA_MANTENIMIENTO,
         tipo = P_TIPO,
         costo = P_COSTO,
         observacion = P_OBSERVACION,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE mantenimiento_id = P_MANTENIMIENTO_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Mantenimiento_Herramienta');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Mantenimiento_Herramienta.mantenimiento_id%TYPE
  ) AS
  BEGIN
    UPDATE Mantenimiento_Herramienta
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE mantenimiento_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Mantenimiento_Herramienta');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Mantenimiento_Herramienta.mantenimiento_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT mantenimiento_id,
             herramienta_id,
             fecha_mantenimiento,
             tipo,
             costo,
             observacion,
             created_at,
             updated_at,
             estado
        FROM Mantenimiento_Herramienta
       WHERE mantenimiento_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT mantenimiento_id,
             herramienta_id,
             fecha_mantenimiento,
             tipo,
             costo,
             observacion,
             created_at,
             updated_at,
             estado
        FROM Mantenimiento_Herramienta
       WHERE estado = 'ACTIVO'
       ORDER BY mantenimiento_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Mantenimiento_Herramienta.fecha_mantenimiento%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT mantenimiento_id,
             herramienta_id,
             fecha_mantenimiento,
             tipo,
             costo,
             observacion,
             created_at,
             updated_at,
             estado
        FROM Mantenimiento_Herramienta
       WHERE UPPER(NVL(TO_CHAR(fecha_mantenimiento), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY mantenimiento_id DESC;
  END SP_BUSCAR;

END PKG_MANTENIMIENTO_HERRAMIENTAS;
/
