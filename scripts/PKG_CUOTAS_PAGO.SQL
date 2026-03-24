CREATE OR REPLACE PACKAGE PKG_CUOTAS_PAGO AS

  PROCEDURE SP_INSERTAR(
    P_PAGO_ID IN Cuotas_Pago.pago_id%TYPE,
    P_NUM_CUOTA IN Cuotas_Pago.num_cuota%TYPE,
    P_MONTO_CUOTA IN Cuotas_Pago.monto_cuota%TYPE,
    P_FECHA_VENCIMIENTO IN Cuotas_Pago.fecha_vencimiento%TYPE,
    P_FECHA_PAGO IN Cuotas_Pago.fecha_pago%TYPE,
    P_ESTADO IN Cuotas_Pago.estado%TYPE,
    P_ID OUT Cuotas_Pago.cuota_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_CUOTA_ID IN Cuotas_Pago.cuota_id%TYPE,
    P_PAGO_ID IN Cuotas_Pago.pago_id%TYPE,
    P_NUM_CUOTA IN Cuotas_Pago.num_cuota%TYPE,
    P_MONTO_CUOTA IN Cuotas_Pago.monto_cuota%TYPE,
    P_FECHA_VENCIMIENTO IN Cuotas_Pago.fecha_vencimiento%TYPE,
    P_FECHA_PAGO IN Cuotas_Pago.fecha_pago%TYPE,
    P_ESTADO IN Cuotas_Pago.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Cuotas_Pago.cuota_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Cuotas_Pago.cuota_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Cuotas_Pago.num_cuota%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_CUOTAS_PAGO;
/
CREATE OR REPLACE PACKAGE BODY PKG_CUOTAS_PAGO AS

  PROCEDURE SP_INSERTAR(
    P_PAGO_ID IN Cuotas_Pago.pago_id%TYPE,
    P_NUM_CUOTA IN Cuotas_Pago.num_cuota%TYPE,
    P_MONTO_CUOTA IN Cuotas_Pago.monto_cuota%TYPE,
    P_FECHA_VENCIMIENTO IN Cuotas_Pago.fecha_vencimiento%TYPE,
    P_FECHA_PAGO IN Cuotas_Pago.fecha_pago%TYPE,
    P_ESTADO IN Cuotas_Pago.estado%TYPE,
    P_ID OUT Cuotas_Pago.cuota_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Pago WHERE pago_id = P_PAGO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Pago para pago_id');
      END IF;
    END;
    INSERT INTO Cuotas_Pago (
      pago_id,
      num_cuota,
      monto_cuota,
      fecha_vencimiento,
      fecha_pago,
      estado,
      created_at
    ) VALUES (
      P_PAGO_ID,
      P_NUM_CUOTA,
      P_MONTO_CUOTA,
      P_FECHA_VENCIMIENTO,
      P_FECHA_PAGO,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING cuota_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_CUOTA_ID IN Cuotas_Pago.cuota_id%TYPE,
    P_PAGO_ID IN Cuotas_Pago.pago_id%TYPE,
    P_NUM_CUOTA IN Cuotas_Pago.num_cuota%TYPE,
    P_MONTO_CUOTA IN Cuotas_Pago.monto_cuota%TYPE,
    P_FECHA_VENCIMIENTO IN Cuotas_Pago.fecha_vencimiento%TYPE,
    P_FECHA_PAGO IN Cuotas_Pago.fecha_pago%TYPE,
    P_ESTADO IN Cuotas_Pago.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Pago WHERE pago_id = P_PAGO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Pago para pago_id');
      END IF;
    END;
    UPDATE Cuotas_Pago
       SET pago_id = P_PAGO_ID,
         num_cuota = P_NUM_CUOTA,
         monto_cuota = P_MONTO_CUOTA,
         fecha_vencimiento = P_FECHA_VENCIMIENTO,
         fecha_pago = P_FECHA_PAGO,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE cuota_id = P_CUOTA_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Cuotas_Pago');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Cuotas_Pago.cuota_id%TYPE
  ) AS
  BEGIN
    UPDATE Cuotas_Pago
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE cuota_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Cuotas_Pago');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Cuotas_Pago.cuota_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT cuota_id,
             pago_id,
             num_cuota,
             monto_cuota,
             fecha_vencimiento,
             fecha_pago,
             created_at,
             updated_at,
             estado
        FROM Cuotas_Pago
       WHERE cuota_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT cuota_id,
             pago_id,
             num_cuota,
             monto_cuota,
             fecha_vencimiento,
             fecha_pago,
             created_at,
             updated_at,
             estado
        FROM Cuotas_Pago
       WHERE estado = 'ACTIVO'
       ORDER BY cuota_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Cuotas_Pago.num_cuota%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT cuota_id,
             pago_id,
             num_cuota,
             monto_cuota,
             fecha_vencimiento,
             fecha_pago,
             created_at,
             updated_at,
             estado
        FROM Cuotas_Pago
       WHERE num_cuota = P_VALOR AND estado = 'ACTIVO'
       ORDER BY cuota_id DESC;
  END SP_BUSCAR;

END PKG_CUOTAS_PAGO;
/
