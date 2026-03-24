CREATE OR REPLACE PACKAGE PKG_INVENTARIO_MATERIA_PRIMA AS

  PROCEDURE SP_INSERTAR(
    P_MP_ID IN Inventario_Materia_Prima.mp_id%TYPE,
    P_STOCK IN Inventario_Materia_Prima.stock%TYPE,
    P_STOCK_MINIMO IN Inventario_Materia_Prima.stock_minimo%TYPE,
    P_ESTADO IN Inventario_Materia_Prima.estado%TYPE,
    P_ID OUT Inventario_Materia_Prima.inv_mp_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_INV_MP_ID IN Inventario_Materia_Prima.inv_mp_id%TYPE,
    P_MP_ID IN Inventario_Materia_Prima.mp_id%TYPE,
    P_STOCK IN Inventario_Materia_Prima.stock%TYPE,
    P_STOCK_MINIMO IN Inventario_Materia_Prima.stock_minimo%TYPE,
    P_ESTADO IN Inventario_Materia_Prima.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Inventario_Materia_Prima.inv_mp_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Inventario_Materia_Prima.inv_mp_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Inventario_Materia_Prima.stock%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_INVENTARIO_MATERIA_PRIMA;
/
CREATE OR REPLACE PACKAGE BODY PKG_INVENTARIO_MATERIA_PRIMA AS

  PROCEDURE SP_INSERTAR(
    P_MP_ID IN Inventario_Materia_Prima.mp_id%TYPE,
    P_STOCK IN Inventario_Materia_Prima.stock%TYPE,
    P_STOCK_MINIMO IN Inventario_Materia_Prima.stock_minimo%TYPE,
    P_ESTADO IN Inventario_Materia_Prima.estado%TYPE,
    P_ID OUT Inventario_Materia_Prima.inv_mp_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Materia_Prima WHERE mp_id = P_MP_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Materia_Prima para mp_id');
      END IF;
    END;
    INSERT INTO Inventario_Materia_Prima (
      mp_id,
      stock,
      stock_minimo,
      estado,
      created_at
    ) VALUES (
      P_MP_ID,
      P_STOCK,
      P_STOCK_MINIMO,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING inv_mp_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_INV_MP_ID IN Inventario_Materia_Prima.inv_mp_id%TYPE,
    P_MP_ID IN Inventario_Materia_Prima.mp_id%TYPE,
    P_STOCK IN Inventario_Materia_Prima.stock%TYPE,
    P_STOCK_MINIMO IN Inventario_Materia_Prima.stock_minimo%TYPE,
    P_ESTADO IN Inventario_Materia_Prima.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Materia_Prima WHERE mp_id = P_MP_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Materia_Prima para mp_id');
      END IF;
    END;
    UPDATE Inventario_Materia_Prima
       SET mp_id = P_MP_ID,
         stock = P_STOCK,
         stock_minimo = P_STOCK_MINIMO,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE inv_mp_id = P_INV_MP_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Inventario_Materia_Prima');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Inventario_Materia_Prima.inv_mp_id%TYPE
  ) AS
  BEGIN
    UPDATE Inventario_Materia_Prima
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE inv_mp_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Inventario_Materia_Prima');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Inventario_Materia_Prima.inv_mp_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT inv_mp_id,
             mp_id,
             stock,
             stock_minimo,
             created_at,
             updated_at,
             estado
        FROM Inventario_Materia_Prima
       WHERE inv_mp_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT inv_mp_id,
             mp_id,
             stock,
             stock_minimo,
             created_at,
             updated_at,
             estado
        FROM Inventario_Materia_Prima
       WHERE estado = 'ACTIVO'
       ORDER BY inv_mp_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Inventario_Materia_Prima.stock%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT inv_mp_id,
             mp_id,
             stock,
             stock_minimo,
             created_at,
             updated_at,
             estado
        FROM Inventario_Materia_Prima
       WHERE UPPER(NVL(TO_CHAR(stock), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY inv_mp_id DESC;
  END SP_BUSCAR;

END PKG_INVENTARIO_MATERIA_PRIMA;
/
