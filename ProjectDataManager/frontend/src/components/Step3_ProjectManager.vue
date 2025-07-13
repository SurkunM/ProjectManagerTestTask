<template>
    <div class="container mt-5">
        <div class="card">
            <div class="card-header">
                <h3>Step 3: Project Manager</h3>
            </div>
            <div class="card-body">
                <div>
                    <div class="mb-3">
                        <label for="manager" class="form-label fs-5">Select Project Manager</label>
                        <v-select v-model="selectedManager"
                                  :options="employees"
                                  :filterable="false"
                                  @search="onSearch"
                                  label="name"
                                  placeholder="Type to search...">
                            <template #no-options>
                                Type to search employees...
                            </template>
                        </v-select>
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
    import vSelect from "vue-select";
    import "vue-select/dist/vue-select.css";

    export default {
        components: { vSelect },
        data() {
            return {
                selectedManager: null
            };

        },

        computed: {
            project() {
                return this.$store.getters.project;
            },

            employees() {
                return this.$store.getters.employees;
            }
        },

        methods: {
            async onSearch(query, loading) {
                if (query.length < 2) return;
                loading(true);
                try {
                    const response = await fetch(`/api/employees?search=${query}`);
                    this.employees = await response.json();
                } catch (error) {
                    console.error("Search failed:", error);
                } finally {
                    loading(false);
                }
            },

            nextStep() {
                if (!this.project.manager) {
                    alert("Please enter the project manager.");

                    return;
                }

                this.$router.push("/executers");
            },

            prevStep() {
                this.$router.push("/companies");
            },
        }
    }
</script>
