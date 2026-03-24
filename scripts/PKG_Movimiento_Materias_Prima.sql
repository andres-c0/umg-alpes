CREATE OR REPLACE PACKAGE PKG_MOVIMIENTO_MATERIAS_PRIMA AS

  PROCEDURE SP_INSERTAR(
    P_INV_MP_ID IN Movimiento_Materia_Prima.inv_mp_id%TYPE,
    P_TIPO_MOV IN Movimiento_Materia_Prima.tipo_mov%TYPE,
    P_CANTIDAD IN Movimiento_Materia_Prima.cantidad%TYPE,
    P_MOTIVO IN Movimiento_Materia_Prima.motivo%TYPE,
    P_MOV_AT IN Movimiento_Materia_Prima.mov_at%TYPE,
    P_ESTADO IN Movimiento_Materia_Prima.estado%TYPE,
    P_ID OUT Movimiento_Materia_Prima.mov_mp_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_MOV_MP_ID IN Movimiento_Materia_Prima.mov_mp_id%TYPE,
    P_INV_MP_ID IN Movimiento_Materia_Prima.inv_mp_id%TYPE,
    P_TIPO_MOV IN Movimiento_Materia_Prima.tipo_mov%TYPE,
    P_CANTIDAD IN Movimiento_Materia_Prima.cantidad%TYPE,
    P_MOTIVO IN Movimiento_Materia_Prima.motivo%TYPE,
    P_MOV_AT IN Movimiento_Materia_Prima.mov_at%TYPE,
    P_ESTADO IN Movimiento_Materia_Prima.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Movimiento_Materia_Prima.mov_mp_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Movimiento_Materia_Prima.mov_mp_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Movimiento_Materia_Prima.tipo_mov%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_MOVIMIENTO_MATERIAS_PRIMA;
/
CREATE OR REPLACE PACKAGE BODY PKG_MOVIMIENTO_MATERIAS_PRIMA AS

  PROCEDURE SP_INSERTAR(
    P_INV_MP_ID IN Movimiento_Materia_Prima.inv_mp_id%TYPE,
    P_TIPO_MOV IN Movimiento_Materia_Prima.tipo_mov%TYPE,
    P_CANTIDAD IN Movimiento_Materia_Prima.cantidad%TYPE,
    P_MOTIVO IN Movimiento_Materia_Prima.motivo%TYPE,
    P_MOV_AT IN Movimiento_Materia_Prima.mov_at%TYPE,
    P_ESTADO IN Movimiento_Materia_Prima.estado%TYPE,
    P_ID OUT Movimiento_Materia_Prima.mov_mp_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Inventario_Materia_Prima WHERE inv_mp_id = P_INV_MP_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Inventario_Materia_Prima para inv_mp_id');
      END IF;
    END;
    INSERT INTO Movimiento_Materia_Prima (
      inv_mp_id,
      tipo_mov,
      cantidad,
      motivo,
      mov_at,
      estado,
      created_at
    ) VALUES (
      P_INV_MP_ID,
      P_TIPO_MOV,
      P_CANTIDAD,
      P_MOTIVO,
      P_MOV_AT,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING mov_mp_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_MOV_MP_ID IN Movimiento_Materia_Prima.mov_mp_id%TYPE,
    P_INV_MP_ID IN Movimiento_Materia_Prima.inv_mp_id%TYPE,
    P_TIPO_MOV IN Movimiento_Materia_Prima.tipo_mov%TYPE,
    P_CANTIDAD IN Movimiento_Materia_Prima.cantidad%TYPE,
    P_MOTIVO IN Movimiento_Materia_Prima.motivo%TYPE,
    P_MOV_AT IN Movimiento_Materia_Prima.mov_at%TYPE,
    P_ESTADO IN Movimiento_Materia_Prima.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Inventario_Materia_Prima WHERE inv_mp_id = P_INV_MP_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Inventario_Materia_Prima para inv_mp_id');
      END IF;
    END;
    UPDATE Movimiento_Materia_Prima
       SET inv_mp_id = P_INV_MP_ID,
         tipo_mov = P_TIPO_MOV,
         cantidad = P_CANTIDAD,
         motivo = P_MOTIVO,
         mov_at = P_MOV_AT,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE mov_mp_id = P_MOV_MP_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Movimiento_Materia_Prima');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Movimiento_Materia_Prima.mov_mp_id%TYPE
  ) AS
  BEGIN
    UPDATE Movimiento_Materia_Prima
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE mov_mp_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Movimiento_Materia_Prima');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Movimiento_Materia_Prima.mov_mp_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT mov_mp_id,
             inv_mp_id,
             tipo_mov,
             cantidad,
             motivo,
             mov_at,
             created_at,
             updated_at,
             estado
        FROM Movimiento_Materia_Prima
       WHERE mov_mp_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT mov_mp_id,
             inv_mp_id,
             tipo_mov,
             cantidad,
             motivo,
             mov_at,
             created_at,
             updated_at,
             estado
        FROM Movimiento_Materia_Prima
       WHERE estado = 'ACTIVO'
       ORDER BY mov_mp_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Movimiento_Materia_Prima.tipo_mov%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT mov_mp_id,
             inv_mp_id,
             tipo_mov,
             cantidad,
             motivo,
             mov_at,
             created_at,
             updated_at,
             estado
        FROM Movimiento_Materia_Prima
       WHERE UPPER(NVL(TO_CHAR(tipo_mov), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY mov_mp_id DESC;
  END SP_BUSCAR;

END PKG_MOVIMIENTO_MATERIAS_PRIMA;
/
