<template>
    <div class="content">
        <div style="margin: 5px;">
            <button data-bs-toggle="modal" @click="itemSelected=[], accion = 'Agregar'" data-bs-target="#exampleModal" class="btn btn-info">Agregar</button>
        </div>
        <table class="table table-striped" style="text-align: center;">
            <thead>
                <tr>
                    <th scope="col">ID</th>
                    <th scope="col">Nombre</th>
                    <th scope="col">Descripcion</th>
                    <th scope="col">Restricción de edad</th>
                    <th scope="col">Compañia</th>
                    <th scope="col">Precio</th>
                    <th scope="col">Acciones</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in items" :key="item.ID">
                    <th scope="row">{{ item.id }}</th>
                    <td>{{ item.nombre }}</td>
                    <td>{{ item.descripcion }}</td>
                    <td>{{ item.restriccionEdad }} Años</td>
                    <td>{{ item.marca }}</td>
                    <td>{{ item.precio }}</td>
                    <td>
                        <button @click='abrirModal( item )' style="margin: 5px;" class="btn btn-warning">Editar</button>
                        <button @click='abrirModaEliminarl( item )' style="margin: 5px;" class="btn btn-danger">Eliminar</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div>
        <!-- Modal -->
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">{{ accion }}: {{itemSelected.nombre}}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <p v-if="errors.length">
                        <b>Por favor, corrija el(los) siguiente(s) error(es):</b>
                        <ul>
                            <li v-for="error in errors">{{ error }}</li>
                            <li v-for="errServer in errorServer">{{ errServer }}</li>
                        </ul>
                    </p>
                    <form id="formulario">
                        <div class="modal-body row">
                            <div class="col-md-12">
                                <input id="id" v-model="itemSelected.id" hidden />
                                <label>Nombre</label>
                                <input id="nombre" v-model="itemSelected.nombre" type="text" class="form-control" required />
                            </div>
                            <div class="col-md-5">
                                <label>Restriccion de edad</label>
                                <input id="restriccionEdad" v-model="itemSelected.restriccionEdad" type="number" class="form-control" />
                            </div>
                            <div class="col-md-4">
                                <label>Marca</label>
                                <input id="marca" v-model="itemSelected.marca" type="text" class="form-control" required />
                            </div>
                            <div class="col-md-3">
                                <label>Precio</label>
                                <input id="precio" v-model="itemSelected.precio" type="number" class="form-control" required />
                            </div>
                            <div class="col-md-12">
                                <label>Descripcion</label>
                                <textarea id="descripcion" class="form-control" required>{{ itemSelected.descripcion }} </textarea>
                            </div>
                        </div>
                    </form>
                    <div class="modal-footer">
                        <button type="button" id="btn-cerrar-modal" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <button type="submit" @click="guardarDatos()" class="btn btn-primary">Guardar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Eliminar -->
    <div class="modal fade" id="modalEliminar" tabindex="-1" aria-labelledby="exampleModalLabel">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Eliminar</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                    <h2>¿Está seguro que decea eliminar el artivulo: {{ itemSelected.nombre }} ?</h2>
                <div class="modal-footer">
                    <button type="button" id="btn-cerrar-modal-eliminar" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button type="submit" @click="eliminarDatos()" class="btn btn-primary">Eliminar</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';

    export default defineComponent({
        data() {
            return {
                modal: false,
                accion: "",
                errorServer: [],
                loading: false,
                errors: [],
                items: [
                    {
                        id: 1,
                        nombre: "Barbie Doctora",
                        descripcion: "Barbie con cosas doctora",
                        restriccionEdad: 3,
                        marca: "Marvel",
                        precio: 500
                    }
                ],
                itemSelected: []
            };
        },
        created() {
                this.loading = true;
                this.obtenerRegistros();
        },
        watch: {
        },
        methods: {
            obtenerRegistros() {

                let ar = this;
                let datos = [];
                fetch('https://localhost:5001/api/Juguetes/GetJuguetes')
                    .then(function (response) {
                        // Transforma la respuesta. En este caso lo convierte a JSON
                        return response.json();
                    })
                    .then(function (data) {
                        ar.items = data;
                        data.forEach(element => datos.push(element));
                        ar.loading = false;

                    });
            },
            abrirModal(item) {
                this.itemSelected = item;
                this.errors = [];
                this.accion = "Modificar";
                var myModal = new bootstrap.Modal(document.getElementById('exampleModal'), {
                    keyboard: false
                })
                myModal.show();
            },
            abrirModaEliminarl(item) {
                this.itemSelected = item;
                this.accion = "Modificar";
                var myModal = new bootstrap.Modal(document.getElementById('modalEliminar'), {
                    keyboard: false
                })
                myModal.show();
            },
            guardarDatos() {
                const ar = this;
                ar.validarDatos();
                if (this.errors.length == 0) {
                    ar.errorServer = [];
                    fetch('https://localhost:5001/api/Juguetes/SaveToy', {
                        method: 'POST',
                        body: JSON.stringify({
                            ID: document.getElementById("id").value == "" ? 0 : document.getElementById("id").value,
                            Nombre: document.getElementById("nombre").value,
                            Descripcion: document.getElementById("descripcion").value,
                            RestriccionEdad: document.getElementById("restriccionEdad").value,
                            Marca: document.getElementById("marca").value,
                            Precio: document.getElementById("precio").value
                        }),
                        headers: {
                            'Accept': "application/json",
                            "Content-Type": "application/json"
                        }
                    })
                        .then(function (response) {
                            if (response.status == 200) {
                                ar.obtenerRegistros();
                                document.getElementById('btn-cerrar-modal').click();
                                ar.itemSelected = [];
                                // Transforma la respuesta. En este caso lo convierte a JSON
                                
                            }
                            return response.json();
                        })
                        .then(function (data) {
                            ar.valdiarErrores(data.errors);
                        });
                }
            },
            valdiarErrores(errors) {
                if (errors != null) {
                    if (errors.Nombre != null)
                        this.errorServer.push(errors.Nombre);
                    if (errors.Descripcion != null)
                        this.errorServer.push(errors.Descripcion);
                    if (errors.RestriccionEdad != null)
                        this.errorServer.push(errors.RestriccionEdad);
                    if (errors.Marca != null)
                        this.errorServer.push(errors.Marca);
                    if (errors.Precio != null)
                        this.errorServer.push(errors.Precio);

                }
            },
            eliminarDatos() {
                const ar = this;
                if (this.errors.length == 0) {
                    this.errors = [];
                    fetch('https://localhost:5001/api/Juguetes/DeleteToy', {
                        method: 'POST',
                        body: JSON.stringify({
                            ID: document.getElementById("id").value == "" ? 0 : document.getElementById("id").value,
                            Nombre: document.getElementById("nombre").value,
                            Descripcion: document.getElementById("descripcion").value,
                            RestriccionEdad: document.getElementById("restriccionEdad").value,
                            Marca: document.getElementById("marca").value,
                            Precio: document.getElementById("precio").value
                        }),
                        headers: {
                            'Accept': "application/json",
                            "Content-Type": "application/json"
                        }
                    })
                        .then(function (response) {
                            if (response.status == 200) {
                                ar.obtenerRegistros();
                                document.getElementById('btn-cerrar-modal-eliminar').click();
                                this.itemSelected = []
                                // Transforma la respuesta. En este caso lo convierte a JSON
                                return response.json();
                            }
                        })
                        .then(function (data) {
                        });
                }
            },
            validarDatos() { 
                this.errors = [];
                let datos = this.itemSelected;
                    if (datos.nombre == null)
                        this.errors.push("Ingrese un nombre.")
                    if (datos.precio == null)
                        this.errors.push("Ingrese el precio del articulo.")
                    if (datos.marca == null)
                        this.errors.push("Ingrese la marca del articulo.")
                
            }
        },
    });
</script>