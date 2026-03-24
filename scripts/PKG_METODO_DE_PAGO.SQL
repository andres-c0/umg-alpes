CREATE OR REPLACE PACKAGE PKG_METODO_DE_PAGO AS

  PROCEDURE SP_INSERTAR(
    P_NOMBRE IN Metodo_Pago.nombre%TYPE,
    P_REQUIERE_DATOS_EXTRA IN Metodo_Pago.requiere_datos_extra%TYPE,
    P_ESTADO IN Metodo_Pago.estado%TYPE,
    P_ID OUT Metodo_Pago.metodo_pago_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_METODO_PAGO_ID IN Metodo_Pago.metodo_pago_id%TYPE,
    P_NOMBRE IN Metodo_Pago.nombre%TYPE,
    P_REQUIERE_DATOS_EXTRA IN Metodo_Pago.requiere_datos_extra%TYPE,
    P_ESTADO IN Metodo_Pago.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Metodo_Pago.metodo_pago_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Metodo_Pago.metodo_pago_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Metodo_Pago.nombre%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_METODO_DE_PAGO;
/
CREATE OR REPLACE PACKAGE BODY PKG_METODO_DE_PAGO AS

  PROCEDURE SP_INSERTAR(
    P_NOMBRE IN Metodo_Pago.nombre%TYPE,
    P_REQUIERE_DATOS_EXTRA IN Metodo_Pago.requiere_datos_extra%TYPE,
    P_ESTADO IN Metodo_Pago.estado%TYPE,
    P_ID OUT Metodo_Pago.metodo_pago_id%TYPE
  ) AS
  BEGIN

    INSERT INTO Metodo_Pago (
      nombre,
      requiere_datos_extra,
      estado,
      created_at
    ) VALUES (
      P_NOMBRE,
      P_REQUIERE_DATOS_EXTRA,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING metodo_pago_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_METODO_PAGO_ID IN Metodo_Pago.metodo_pago_id%TYPE,
    P_NOMBRE IN Metodo_Pago.nombre%TYPE,
    P_REQUIERE_DATOS_EXTRA IN Metodo_Pago.requiere_datos_extra%TYPE,
    P_ESTADO IN Metodo_Pago.estado%TYPE
  ) AS
  BEGIN

    UPDATE Metodo_Pago
       SET nombre = P_NOMBRE,
         requiere_datos_extra = P_REQUIERE_DATOS_EXTRA,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE metodo_pago_id = P_METODO_PAGO_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Metodo_Pago');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Metodo_Pago.metodo_pago_id%TYPE
  ) AS
  BEGIN
    UPDATE Metodo_Pago
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE metodo_pago_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Metodo_Pago');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Metodo_Pago.metodo_pago_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT metodo_pago_id,
             nombre,
             requiere_datos_extra,
             created_at,
             updated_at,
             estado
        FROM Metodo_Pago
       WHERE metodo_pago_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT metodo_pago_id,
             nombre,
             requiere_datos_extra,
             created_at,
             updated_at,
             estado
        FROM Metodo_Pago
       WHERE estado = 'ACTIVO'
       ORDER BY metodo_pago_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Metodo_Pago.nombre%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT metodo_pago_id,
             nombre,
             requiere_datos_extra,
             created_at,
             updated_at,
             estado
        FROM Metodo_Pago
       WHERE UPPER(NVL(TO_CHAR(nombre), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY metodo_pago_id DESC;
  END SP_BUSCAR;

END PKG_METODO_DE_PAGO;
/
