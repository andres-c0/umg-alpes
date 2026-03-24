CREATE OR REPLACE PACKAGE PKG_ORDEN_VENTA AS

  PROCEDURE SP_INSERTAR(
    P_NUM_ORDEN IN Orden_Venta.num_orden%TYPE,
    P_CLI_ID IN Orden_Venta.cli_id%TYPE,
    P_ESTADO_ORDEN_ID IN Orden_Venta.estado_orden_id%TYPE,
    P_FECHA_ORDEN IN Orden_Venta.fecha_orden%TYPE,
    P_SUBTOTAL IN Orden_Venta.subtotal%TYPE,
    P_DESCUENTO IN Orden_Venta.descuento%TYPE,
    P_IMPUESTO IN Orden_Venta.impuesto%TYPE,
    P_TOTAL IN Orden_Venta.total%TYPE,
    P_MONEDA IN Orden_Venta.moneda%TYPE,
    P_DIRECCION_ENVIO_SNAPSHOT IN Orden_Venta.direccion_envio_snapshot%TYPE,
    P_OBSERVACIONES IN Orden_Venta.observaciones%TYPE,
    P_ESTADO IN Orden_Venta.estado%TYPE,
    P_ID OUT Orden_Venta.orden_venta_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_ORDEN_VENTA_ID IN Orden_Venta.orden_venta_id%TYPE,
    P_NUM_ORDEN IN Orden_Venta.num_orden%TYPE,
    P_CLI_ID IN Orden_Venta.cli_id%TYPE,
    P_ESTADO_ORDEN_ID IN Orden_Venta.estado_orden_id%TYPE,
    P_FECHA_ORDEN IN Orden_Venta.fecha_orden%TYPE,
    P_SUBTOTAL IN Orden_Venta.subtotal%TYPE,
    P_DESCUENTO IN Orden_Venta.descuento%TYPE,
    P_IMPUESTO IN Orden_Venta.impuesto%TYPE,
    P_TOTAL IN Orden_Venta.total%TYPE,
    P_MONEDA IN Orden_Venta.moneda%TYPE,
    P_DIRECCION_ENVIO_SNAPSHOT IN Orden_Venta.direccion_envio_snapshot%TYPE,
    P_OBSERVACIONES IN Orden_Venta.observaciones%TYPE,
    P_ESTADO IN Orden_Venta.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Orden_Venta.orden_venta_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Orden_Venta.orden_venta_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Orden_Venta.num_orden%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_ORDEN_VENTA;
/
CREATE OR REPLACE PACKAGE BODY PKG_ORDEN_VENTA AS

  PROCEDURE SP_INSERTAR(
    P_NUM_ORDEN IN Orden_Venta.num_orden%TYPE,
    P_CLI_ID IN Orden_Venta.cli_id%TYPE,
    P_ESTADO_ORDEN_ID IN Orden_Venta.estado_orden_id%TYPE,
    P_FECHA_ORDEN IN Orden_Venta.fecha_orden%TYPE,
    P_SUBTOTAL IN Orden_Venta.subtotal%TYPE,
    P_DESCUENTO IN Orden_Venta.descuento%TYPE,
    P_IMPUESTO IN Orden_Venta.impuesto%TYPE,
    P_TOTAL IN Orden_Venta.total%TYPE,
    P_MONEDA IN Orden_Venta.moneda%TYPE,
    P_DIRECCION_ENVIO_SNAPSHOT IN Orden_Venta.direccion_envio_snapshot%TYPE,
    P_OBSERVACIONES IN Orden_Venta.observaciones%TYPE,
    P_ESTADO IN Orden_Venta.estado%TYPE,
    P_ID OUT Orden_Venta.orden_venta_id%TYPE
  ) AS
  BEGIN
    IF P_CLI_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Cliente WHERE cli_id = P_CLI_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Cliente para cli_id');
        END IF;
      END;
    END IF;
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Estado_Orden WHERE estado_orden_id = P_ESTADO_ORDEN_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Estado_Orden para estado_orden_id');
      END IF;
    END;
    INSERT INTO Orden_Venta (
      num_orden,
      cli_id,
      estado_orden_id,
      fecha_orden,
      subtotal,
      descuento,
      impuesto,
      total,
      moneda,
      direccion_envio_snapshot,
      observaciones,
      estado,
      created_at
    ) VALUES (
      P_NUM_ORDEN,
      P_CLI_ID,
      P_ESTADO_ORDEN_ID,
      P_FECHA_ORDEN,
      P_SUBTOTAL,
      P_DESCUENTO,
      P_IMPUESTO,
      P_TOTAL,
      P_MONEDA,
      P_DIRECCION_ENVIO_SNAPSHOT,
      P_OBSERVACIONES,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING orden_venta_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_ORDEN_VENTA_ID IN Orden_Venta.orden_venta_id%TYPE,
    P_NUM_ORDEN IN Orden_Venta.num_orden%TYPE,
    P_CLI_ID IN Orden_Venta.cli_id%TYPE,
    P_ESTADO_ORDEN_ID IN Orden_Venta.estado_orden_id%TYPE,
    P_FECHA_ORDEN IN Orden_Venta.fecha_orden%TYPE,
    P_SUBTOTAL IN Orden_Venta.subtotal%TYPE,
    P_DESCUENTO IN Orden_Venta.descuento%TYPE,
    P_IMPUESTO IN Orden_Venta.impuesto%TYPE,
    P_TOTAL IN Orden_Venta.total%TYPE,
    P_MONEDA IN Orden_Venta.moneda%TYPE,
    P_DIRECCION_ENVIO_SNAPSHOT IN Orden_Venta.direccion_envio_snapshot%TYPE,
    P_OBSERVACIONES IN Orden_Venta.observaciones%TYPE,
    P_ESTADO IN Orden_Venta.estado%TYPE
  ) AS
  BEGIN
    IF P_CLI_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Cliente WHERE cli_id = P_CLI_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Cliente para cli_id');
        END IF;
      END;
    END IF;
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Estado_Orden WHERE estado_orden_id = P_ESTADO_ORDEN_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Estado_Orden para estado_orden_id');
      END IF;
    END;
    UPDATE Orden_Venta
       SET num_orden = P_NUM_ORDEN,
         cli_id = P_CLI_ID,
         estado_orden_id = P_ESTADO_ORDEN_ID,
         fecha_orden = P_FECHA_ORDEN,
         subtotal = P_SUBTOTAL,
         descuento = P_DESCUENTO,
         impuesto = P_IMPUESTO,
         total = P_TOTAL,
         moneda = P_MONEDA,
         direccion_envio_snapshot = P_DIRECCION_ENVIO_SNAPSHOT,
         observaciones = P_OBSERVACIONES,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE orden_venta_id = P_ORDEN_VENTA_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Orden_Venta');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Orden_Venta.orden_venta_id%TYPE
  ) AS
  BEGIN
    UPDATE Orden_Venta
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE orden_venta_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Orden_Venta');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Orden_Venta.orden_venta_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT orden_venta_id,
             num_orden,
             cli_id,
             estado_orden_id,
             fecha_orden,
             subtotal,
             descuento,
             impuesto,
             total,
             moneda,
             direccion_envio_snapshot,
             observaciones,
             created_at,
             updated_at,
             estado
        FROM Orden_Venta
       WHERE orden_venta_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT orden_venta_id,
             num_orden,
             cli_id,
             estado_orden_id,
             fecha_orden,
             subtotal,
             descuento,
             impuesto,
             total,
             moneda,
             direccion_envio_snapshot,
             observaciones,
             created_at,
             updated_at,
             estado
        FROM Orden_Venta
       WHERE estado = 'ACTIVO'
       ORDER BY orden_venta_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Orden_Venta.num_orden%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT orden_venta_id,
             num_orden,
             cli_id,
             estado_orden_id,
             fecha_orden,
             subtotal,
             descuento,
             impuesto,
             total,
             moneda,
             direccion_envio_snapshot,
             observaciones,
             created_at,
             updated_at,
             estado
        FROM Orden_Venta
       WHERE UPPER(NVL(TO_CHAR(num_orden), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY orden_venta_id DESC;
  END SP_BUSCAR;

END PKG_ORDEN_VENTA;
/
