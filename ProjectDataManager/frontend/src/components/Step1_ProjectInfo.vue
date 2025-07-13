<template>
    <div class="container mt-5">
        <div class="card">
            <div class="card-header">
                <h4>Step 1: Project Details</h4>
            </div>
            <div class="card-body">
                <div>
                    <div class="mb-3">
                        <label for="project" class="form-label fs-5">Project name</label>
                        <input type="text"
                               class="form-control"
                               id="project"
                               v-model.trim="project.name"
                               autocomplete="off">
                    </div>
                    <div class="row mb-3">
                        <div class="col">
                            <label for="startDate" class="form-label fs-5">Start date</label>
                            <input type="date" class="form-control" id="startDate" v-model.trim="project.startDate">
                        </div>
                        <div class="col">
                            <label for="endDate" class="form-label fs-5">End date</label>
                            <input type="date" class="form-control" id="endDate" v-model.trim="project.endDate">
                        </div>
                    </div>
                    <div class="mb-3">
                        <label for="priority" class="form-label fs-5">Priority</label>
                        <select class="form-select" id="priority" v-model.trim="project.priority">
                            <option value="1">Low</option>
                            <option value="2">Middle</option>
                            <option value="3">High</option>
                        </select>
                    </div>

                    <div class="d-flex justify-content-end mt-4">
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
    export default {
        computed: {
            project() {
                return this.$store.getters.project;
            }
        },

        methods: {
            isValidFields() {
                if (!this.project.name) {
                    alert("Please enter the project name.");

                    return false;
                }

                if (!this.project.startDate || !this.project.endDate) {
                    alert("Please specify the start and end dates.");

                    return false;
                }

                if (new Date(this.project.startDate) > new Date(this.project.endDate)) {
                    alert("The start date cannot be later than the end date.");

                    return false;
                }

                return true;
            },

            nextStep() {
                if (!this.isValidFields()) {
                    return;
                }

                this.$router.push("/companies");
            }
        }
    }
</script>