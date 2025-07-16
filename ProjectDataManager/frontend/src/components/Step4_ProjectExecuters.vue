<template>
    <div class="container mt-5">
        <div class="card">
            <div class="card-header">
                <h3>Step 4: Executers</h3>
            </div>
            <div class="card-body">
                <div>
                    <div class="mb-3">
                        <label for="executersSearch" class="form-label fs-5">
                            Employee
                            <span v-if="isLoading" class="text-muted">Searching...</span>
                        </label>
                        <input type="text"
                               class="form-control mb-2"
                               placeholder="search..."
                               v-model="term"
                               id="executersSearch"
                               autocomplete="off"
                               @input="debouncedSearch">

                        <select id="executerSelect"
                                class="form-select"
                                @change="addSelectedExecuter($event.target.value)">
                            <option disabled selected value="">Choose an executer</option>
                            <option v-for="employee in employees"
                                    :key="employee.id"
                                    :value="employee.id">
                                {{ employee.firstName }} {{ employee.lastName }} {{ employee.middleName }}
                            </option>
                        </select>

                        <label v-if="executers.length > 0" class="form-label fs-5 mt-2">Selected executers</label>
                        <ul class="list-group">
                            <li class="list-group-item d-flex justify-content-between "
                                v-for="executer in executers"
                                :key="executer.id">
                                {{ executer.firstName }} {{ executer.lastName }} {{ executer.middleName }}
                                <button class="btn btn-danger btn-sm ms-auto"
                                        @click="deleteSelectedExecuter(executer.id)">
                                    Delete
                                </button>
                            </li>
                        </ul>
                    </div>
                </div>

                <div class="d-flex justify-content-between  mt-4">
                    <button class="btn btn-secondary px-4" @click="prevStep">
                        Back
                    </button>
                    <button class="btn btn-primary px-4" @click="nextStep">
                        Next
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import debounce from "lodash.debounce";

    export default {
        data() {
            return {
                term: "",
                debounceTimeout: null
            };
        },

        created() {
            this.$store.commit("setTerm", this.term);

            this.$store.dispatch("loadEmployees")
                .catch(() => {
                    alert("Error! Failed to upload");
                });
        },

        computed: {
            employees() {
                return this.$store.getters.employees;
            },

            executers() {
                return this.$store.getters.executers;
            },

            isLoading() {
                return this.$store.getters.isLoading;
            }
        },

        methods: {
            addSelectedExecuter(valueId) {
                const id = parseInt(valueId);

                if (this.executers.some(e => e.id === id)) {
                    return;
                }

                const executer = this.employees.find(e => e.id === id);
                this.$store.commit("setExecuter", executer);
            },

            deleteSelectedExecuter(id) {
                this.$store.commit("deleteExecuter", { id });
            },

            nextStep() {
                if (this.executers.length === 0) {
                    alert("Please identify the project executers.");
                    return;
                }

                this.$router.push("/documents");
            },

            prevStep() {
                this.$router.push("/manager");
            },

            debouncedSearch: debounce(function () {
                this.$store.commit("setTerm", this.term);

                this.$store.dispatch("loadEmployees")
                    .catch(() => {
                        alert("Error! Failed to upload");
                    });
            }, 300)
        }
    }
</script>
