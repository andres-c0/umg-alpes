CREATE OR REPLACE PACKAGE PKG_MATERIA_PRIMA AS

  PROCEDURE SP_INSERTAR(
    P_CODIGO IN Materia_Prima.codigo%TYPE,
    P_NOMBRE IN Materia_Prima.nombre%TYPE,
    P_UNIDAD_MEDIDA_ID IN Materia_Prima.unidad_medida_id%TYPE,
    P_DESCRIPCION IN Materia_Prima.descripcion%TYPE,
    P_ESTADO IN Materia_Prima.estado%TYPE,
    P_ID OUT Materia_Prima.mp_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_MP_ID IN Materia_Prima.mp_id%TYPE,
    P_CODIGO IN Materia_Prima.codigo%TYPE,
    P_NOMBRE IN Materia_Prima.nombre%TYPE,
    P_UNIDAD_MEDIDA_ID IN Materia_Prima.unidad_medida_id%TYPE,
    P_DESCRIPCION IN Materia_Prima.descripcion%TYPE,
    P_ESTADO IN Materia_Prima.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Materia_Prima.mp_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Materia_Prima.mp_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Materia_Prima.codigo%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_MATERIA_PRIMA;
/
CREATE OR REPLACE PACKAGE BODY PKG_MATERIA_PRIMA AS

  PROCEDURE SP_INSERTAR(
    P_CODIGO IN Materia_Prima.codigo%TYPE,
    P_NOMBRE IN Materia_Prima.nombre%TYPE,
    P_UNIDAD_MEDIDA_ID IN Materia_Prima.unidad_medida_id%TYPE,
    P_DESCRIPCION IN Materia_Prima.descripcion%TYPE,
    P_ESTADO IN Materia_Prima.estado%TYPE,
    P_ID OUT Materia_Prima.mp_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Unidad_Medida WHERE unidad_medida_id = P_UNIDAD_MEDIDA_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Unidad_Medida para unidad_medida_id');
      END IF;
    END;
    INSERT INTO Materia_Prima (
      codigo,
      nombre,
      unidad_medida_id,
      descripcion,
      estado,
      created_at
    ) VALUES (
      P_CODIGO,
      P_NOMBRE,
      P_UNIDAD_MEDIDA_ID,
      P_DESCRIPCION,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING mp_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_MP_ID IN Materia_Prima.mp_id%TYPE,
    P_CODIGO IN Materia_Prima.codigo%TYPE,
    P_NOMBRE IN Materia_Prima.nombre%TYPE,
    P_UNIDAD_MEDIDA_ID IN Materia_Prima.unidad_medida_id%TYPE,
    P_DESCRIPCION IN Materia_Prima.descripcion%TYPE,
    P_ESTADO IN Materia_Prima.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Unidad_Medida WHERE unidad_medida_id = P_UNIDAD_MEDIDA_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Unidad_Medida para unidad_medida_id');
      END IF;
    END;
    UPDATE Materia_Prima
       SET codigo = P_CODIGO,
         nombre = P_NOMBRE,
         unidad_medida_id = P_UNIDAD_MEDIDA_ID,
         descripcion = P_DESCRIPCION,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE mp_id = P_MP_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Materia_Prima');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Materia_Prima.mp_id%TYPE
  ) AS
  BEGIN
    UPDATE Materia_Prima
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE mp_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Materia_Prima');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Materia_Prima.mp_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT mp_id,
             codigo,
             nombre,
             unidad_medida_id,
             descripcion,
             created_at,
             updated_at,
             estado
        FROM Materia_Prima
       WHERE mp_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT mp_id,
             codigo,
             nombre,
             unidad_medida_id,
             descripcion,
             created_at,
             updated_at,
             estado
        FROM Materia_Prima
       WHERE estado = 'ACTIVO'
       ORDER BY mp_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Materia_Prima.codigo%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT mp_id,
             codigo,
             nombre,
             unidad_medida_id,
             descripcion,
             created_at,
             updated_at,
             estado
        FROM Materia_Prima
       WHERE UPPER(NVL(TO_CHAR(codigo), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY mp_id DESC;
  END SP_BUSCAR;

END PKG_MATERIA_PRIMA;
/
