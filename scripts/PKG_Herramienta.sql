CREATE OR REPLACE PACKAGE PKG_HERRAMIENTA AS

  PROCEDURE SP_INSERTAR(
    P_CODIGO IN Herramienta.codigo%TYPE,
    P_NOMBRE IN Herramienta.nombre%TYPE,
    P_DESCRIPCION IN Herramienta.descripcion%TYPE,
    P_FECHA_COMPRA IN Herramienta.fecha_compra%TYPE,
    P_ESTADO IN Herramienta.estado%TYPE,
    P_ID OUT Herramienta.herramienta_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_HERRAMIENTA_ID IN Herramienta.herramienta_id%TYPE,
    P_CODIGO IN Herramienta.codigo%TYPE,
    P_NOMBRE IN Herramienta.nombre%TYPE,
    P_DESCRIPCION IN Herramienta.descripcion%TYPE,
    P_FECHA_COMPRA IN Herramienta.fecha_compra%TYPE,
    P_ESTADO IN Herramienta.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Herramienta.herramienta_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Herramienta.herramienta_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Herramienta.codigo%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_HERRAMIENTA;
/
CREATE OR REPLACE PACKAGE BODY PKG_HERRAMIENTA AS

  PROCEDURE SP_INSERTAR(
    P_CODIGO IN Herramienta.codigo%TYPE,
    P_NOMBRE IN Herramienta.nombre%TYPE,
    P_DESCRIPCION IN Herramienta.descripcion%TYPE,
    P_FECHA_COMPRA IN Herramienta.fecha_compra%TYPE,
    P_ESTADO IN Herramienta.estado%TYPE,
    P_ID OUT Herramienta.herramienta_id%TYPE
  ) AS
  BEGIN

    INSERT INTO Herramienta (
      codigo,
      nombre,
      descripcion,
      fecha_compra,
      estado,
      created_at
    ) VALUES (
      P_CODIGO,
      P_NOMBRE,
      P_DESCRIPCION,
      P_FECHA_COMPRA,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING herramienta_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_HERRAMIENTA_ID IN Herramienta.herramienta_id%TYPE,
    P_CODIGO IN Herramienta.codigo%TYPE,
    P_NOMBRE IN Herramienta.nombre%TYPE,
    P_DESCRIPCION IN Herramienta.descripcion%TYPE,
    P_FECHA_COMPRA IN Herramienta.fecha_compra%TYPE,
    P_ESTADO IN Herramienta.estado%TYPE
  ) AS
  BEGIN

    UPDATE Herramienta
       SET codigo = P_CODIGO,
         nombre = P_NOMBRE,
         descripcion = P_DESCRIPCION,
         fecha_compra = P_FECHA_COMPRA,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE herramienta_id = P_HERRAMIENTA_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Herramienta');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Herramienta.herramienta_id%TYPE
  ) AS
  BEGIN
    UPDATE Herramienta
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE herramienta_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Herramienta');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Herramienta.herramienta_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT herramienta_id,
             codigo,
             nombre,
             descripcion,
             fecha_compra,
             created_at,
             updated_at,
             estado
        FROM Herramienta
       WHERE herramienta_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT herramienta_id,
             codigo,
             nombre,
             descripcion,
             fecha_compra,
             created_at,
             updated_at,
             estado
        FROM Herramienta
       WHERE estado = 'ACTIVO'
       ORDER BY herramienta_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Herramienta.codigo%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT herramienta_id,
             codigo,
             nombre,
             descripcion,
             fecha_compra,
             created_at,
             updated_at,
             estado
        FROM Herramienta
       WHERE UPPER(NVL(TO_CHAR(codigo), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY herramienta_id DESC;
  END SP_BUSCAR;

END PKG_HERRAMIENTA;
/
