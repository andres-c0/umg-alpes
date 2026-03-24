CREATE OR REPLACE PACKAGE PKG_PAGO AS

  PROCEDURE SP_INSERTAR(
    P_ORDEN_VENTA_ID IN Pago.orden_venta_id%TYPE,
    P_METODO_PAGO_ID IN Pago.metodo_pago_id%TYPE,
    P_MONTO IN Pago.monto%TYPE,
    P_ESTADO_PAGO IN Pago.estado_pago%TYPE,
    P_REFERENCIA IN Pago.referencia%TYPE,
    P_PAGO_AT IN Pago.pago_at%TYPE,
    P_ESTADO IN Pago.estado%TYPE,
    P_ID OUT Pago.pago_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_PAGO_ID IN Pago.pago_id%TYPE,
    P_ORDEN_VENTA_ID IN Pago.orden_venta_id%TYPE,
    P_METODO_PAGO_ID IN Pago.metodo_pago_id%TYPE,
    P_MONTO IN Pago.monto%TYPE,
    P_ESTADO_PAGO IN Pago.estado_pago%TYPE,
    P_REFERENCIA IN Pago.referencia%TYPE,
    P_PAGO_AT IN Pago.pago_at%TYPE,
    P_ESTADO IN Pago.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Pago.pago_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Pago.pago_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Pago.monto%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_PAGO;
/
CREATE OR REPLACE PACKAGE BODY PKG_PAGO AS

  PROCEDURE SP_INSERTAR(
    P_ORDEN_VENTA_ID IN Pago.orden_venta_id%TYPE,
    P_METODO_PAGO_ID IN Pago.metodo_pago_id%TYPE,
    P_MONTO IN Pago.monto%TYPE,
    P_ESTADO_PAGO IN Pago.estado_pago%TYPE,
    P_REFERENCIA IN Pago.referencia%TYPE,
    P_PAGO_AT IN Pago.pago_at%TYPE,
    P_ESTADO IN Pago.estado%TYPE,
    P_ID OUT Pago.pago_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Orden_Venta WHERE orden_venta_id = P_ORDEN_VENTA_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Orden_Venta para orden_venta_id');
      END IF;
    END;
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Metodo_Pago WHERE metodo_pago_id = P_METODO_PAGO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Metodo_Pago para metodo_pago_id');
      END IF;
    END;
    INSERT INTO Pago (
      orden_venta_id,
      metodo_pago_id,
      monto,
      estado_pago,
      referencia,
      pago_at,
      estado,
      created_at
    ) VALUES (
      P_ORDEN_VENTA_ID,
      P_METODO_PAGO_ID,
      P_MONTO,
      P_ESTADO_PAGO,
      P_REFERENCIA,
      P_PAGO_AT,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING pago_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_PAGO_ID IN Pago.pago_id%TYPE,
    P_ORDEN_VENTA_ID IN Pago.orden_venta_id%TYPE,
    P_METODO_PAGO_ID IN Pago.metodo_pago_id%TYPE,
    P_MONTO IN Pago.monto%TYPE,
    P_ESTADO_PAGO IN Pago.estado_pago%TYPE,
    P_REFERENCIA IN Pago.referencia%TYPE,
    P_PAGO_AT IN Pago.pago_at%TYPE,
    P_ESTADO IN Pago.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Orden_Venta WHERE orden_venta_id = P_ORDEN_VENTA_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Orden_Venta para orden_venta_id');
      END IF;
    END;
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Metodo_Pago WHERE metodo_pago_id = P_METODO_PAGO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Metodo_Pago para metodo_pago_id');
      END IF;
    END;
    UPDATE Pago
       SET orden_venta_id = P_ORDEN_VENTA_ID,
         metodo_pago_id = P_METODO_PAGO_ID,
         monto = P_MONTO,
         estado_pago = P_ESTADO_PAGO,
         referencia = P_REFERENCIA,
         pago_at = P_PAGO_AT,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE pago_id = P_PAGO_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Pago');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Pago.pago_id%TYPE
  ) AS
  BEGIN
    UPDATE Pago
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE pago_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Pago');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Pago.pago_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT pago_id,
             orden_venta_id,
             metodo_pago_id,
             monto,
             estado_pago,
             referencia,
             pago_at,
             created_at,
             updated_at,
             estado
        FROM Pago
       WHERE pago_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT pago_id,
             orden_venta_id,
             metodo_pago_id,
             monto,
             estado_pago,
             referencia,
             pago_at,
             created_at,
             updated_at,
             estado
        FROM Pago
       WHERE estado = 'ACTIVO'
       ORDER BY pago_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Pago.monto%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT pago_id,
             orden_venta_id,
             metodo_pago_id,
             monto,
             estado_pago,
             referencia,
             pago_at,
             created_at,
             updated_at,
             estado
        FROM Pago
       WHERE UPPER(NVL(TO_CHAR(monto), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY pago_id DESC;
  END SP_BUSCAR;

END PKG_PAGO;
/
