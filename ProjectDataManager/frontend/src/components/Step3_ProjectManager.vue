<template>
    <div class="container mt-5">
        <div class="card">
            <div class="card-header">
                <h3>Step 3: Project Manager</h3>
            </div>
            <div class="card-body">
                <div>

                    <div class="mb-3">
                        <label for="manager" class="form-label fs-5">
                            Manager
                            <span v-if="isLoading" class="text-muted">Searching...</span>
                        </label>
                        <input type="text"
                               class="form-control mb-2"
                               placeholder="search..."
                               v-model="trim"
                               @input="debouncedSearch">

                        <select class="form-select"
                                v-model="project.manager">
                            <option disabled selected value="">Choose an manager</option>

                            <option v-for="employee in employees"
                                    :key="employee.id"
                                    :value="employee.id">
                                {{ employee.firstName }} {{ employee.lasName }} {{ employee.middleName }}
                            </option>
                        </select>
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
    </div>
</template>
<script>
    import debounce from "lodash.debounce";

    export default {
        data() {
            return {
                trim: "",
                debounceTimeout: null,
            };
        },

        computed: {
            project() {
                return this.$store.getters.project;
            },

            employees() {
                return this.$store.getters.employees;
            },

            isLoading() {
                return this.$store.getters.isLoading;
            }
        },

        methods: {
            nextStep() {
                if (!this.project.manager) {
                    alert("Please identify the project manager.");
                    return;
                }

                this.$router.push("/executers");
            },

            prevStep() {
                this.$router.push("/companies");
            },

            debouncedSearch: debounce(function () {
                //this.loadEmployees(this.searchQuery);

                this.$store.dispatch("loadContacts")
                    .catch(() => {
                        alert("Error! Failed to upload");
                    });
            }, 300),
        }
    }
</script>
