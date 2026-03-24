CREATE OR REPLACE PACKAGE PKG_ORDEN_VENTA_DETALLE AS

  PROCEDURE SP_INSERTAR(
    P_ORDEN_VENTA_ID IN Orden_Venta_Detalle.orden_venta_id%TYPE,
    P_PRODUCTO_ID IN Orden_Venta_Detalle.producto_id%TYPE,
    P_CANTIDAD IN Orden_Venta_Detalle.cantidad%TYPE,
    P_PRECIO_UNITARIO_SNAPSHOT IN Orden_Venta_Detalle.precio_unitario_snapshot%TYPE,
    P_SUBTOTAL_LINEA IN Orden_Venta_Detalle.subtotal_linea%TYPE,
    P_ESTADO IN Orden_Venta_Detalle.estado%TYPE,
    P_ID OUT Orden_Venta_Detalle.orden_venta_det_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_ORDEN_VENTA_DET_ID IN Orden_Venta_Detalle.orden_venta_det_id%TYPE,
    P_ORDEN_VENTA_ID IN Orden_Venta_Detalle.orden_venta_id%TYPE,
    P_PRODUCTO_ID IN Orden_Venta_Detalle.producto_id%TYPE,
    P_CANTIDAD IN Orden_Venta_Detalle.cantidad%TYPE,
    P_PRECIO_UNITARIO_SNAPSHOT IN Orden_Venta_Detalle.precio_unitario_snapshot%TYPE,
    P_SUBTOTAL_LINEA IN Orden_Venta_Detalle.subtotal_linea%TYPE,
    P_ESTADO IN Orden_Venta_Detalle.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Orden_Venta_Detalle.orden_venta_det_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Orden_Venta_Detalle.orden_venta_det_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Orden_Venta_Detalle.cantidad%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_ORDEN_VENTA_DETALLE;
/
CREATE OR REPLACE PACKAGE BODY PKG_ORDEN_VENTA_DETALLE AS

  PROCEDURE SP_INSERTAR(
    P_ORDEN_VENTA_ID IN Orden_Venta_Detalle.orden_venta_id%TYPE,
    P_PRODUCTO_ID IN Orden_Venta_Detalle.producto_id%TYPE,
    P_CANTIDAD IN Orden_Venta_Detalle.cantidad%TYPE,
    P_PRECIO_UNITARIO_SNAPSHOT IN Orden_Venta_Detalle.precio_unitario_snapshot%TYPE,
    P_SUBTOTAL_LINEA IN Orden_Venta_Detalle.subtotal_linea%TYPE,
    P_ESTADO IN Orden_Venta_Detalle.estado%TYPE,
    P_ID OUT Orden_Venta_Detalle.orden_venta_det_id%TYPE
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
      SELECT COUNT(*) INTO V_EXISTE FROM Producto WHERE producto_id = P_PRODUCTO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Producto para producto_id');
      END IF;
    END;
    INSERT INTO Orden_Venta_Detalle (
      orden_venta_id,
      producto_id,
      cantidad,
      precio_unitario_snapshot,
      subtotal_linea,
      estado,
      created_at
    ) VALUES (
      P_ORDEN_VENTA_ID,
      P_PRODUCTO_ID,
      P_CANTIDAD,
      P_PRECIO_UNITARIO_SNAPSHOT,
      P_SUBTOTAL_LINEA,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING orden_venta_det_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_ORDEN_VENTA_DET_ID IN Orden_Venta_Detalle.orden_venta_det_id%TYPE,
    P_ORDEN_VENTA_ID IN Orden_Venta_Detalle.orden_venta_id%TYPE,
    P_PRODUCTO_ID IN Orden_Venta_Detalle.producto_id%TYPE,
    P_CANTIDAD IN Orden_Venta_Detalle.cantidad%TYPE,
    P_PRECIO_UNITARIO_SNAPSHOT IN Orden_Venta_Detalle.precio_unitario_snapshot%TYPE,
    P_SUBTOTAL_LINEA IN Orden_Venta_Detalle.subtotal_linea%TYPE,
    P_ESTADO IN Orden_Venta_Detalle.estado%TYPE
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
      SELECT COUNT(*) INTO V_EXISTE FROM Producto WHERE producto_id = P_PRODUCTO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Producto para producto_id');
      END IF;
    END;
    UPDATE Orden_Venta_Detalle
       SET orden_venta_id = P_ORDEN_VENTA_ID,
         producto_id = P_PRODUCTO_ID,
         cantidad = P_CANTIDAD,
         precio_unitario_snapshot = P_PRECIO_UNITARIO_SNAPSHOT,
         subtotal_linea = P_SUBTOTAL_LINEA,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE orden_venta_det_id = P_ORDEN_VENTA_DET_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Orden_Venta_Detalle');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Orden_Venta_Detalle.orden_venta_det_id%TYPE
  ) AS
  BEGIN
    UPDATE Orden_Venta_Detalle
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE orden_venta_det_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Orden_Venta_Detalle');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Orden_Venta_Detalle.orden_venta_det_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT orden_venta_det_id,
             orden_venta_id,
             producto_id,
             cantidad,
             precio_unitario_snapshot,
             subtotal_linea,
             created_at,
             updated_at,
             estado
        FROM Orden_Venta_Detalle
       WHERE orden_venta_det_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT orden_venta_det_id,
             orden_venta_id,
             producto_id,
             cantidad,
             precio_unitario_snapshot,
             subtotal_linea,
             created_at,
             updated_at,
             estado
        FROM Orden_Venta_Detalle
       WHERE estado = 'ACTIVO'
       ORDER BY orden_venta_det_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Orden_Venta_Detalle.cantidad%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT orden_venta_det_id,
             orden_venta_id,
             producto_id,
             cantidad,
             precio_unitario_snapshot,
             subtotal_linea,
             created_at,
             updated_at,
             estado
        FROM Orden_Venta_Detalle
       WHERE cantidad = P_VALOR AND estado = 'ACTIVO'
       ORDER BY orden_venta_det_id DESC;
  END SP_BUSCAR;

END PKG_ORDEN_VENTA_DETALLE;
/
