<template>
    <div class="container mt-5">
        <div class="card">
            <div class="card-header">
                <h3>Step 2: Companies</h3>
            </div>
            <div class="card-body">
                <div>
                    <div class="mb-3">
                        <label for="customer" class="form-label fs-5">Customer</label>
                        <input type="text"
                               class="form-control"
                               id="customer"
                               v-model.trim="project.customerCompany"
                               autocomplete="off">
                    </div>
                    <div class="mb-3">
                        <label for="contractor" class="form-label fs-5">Contractor</label>
                        <input type="text"
                               class="form-control"
                               id="contractor"
                               v-model.trim="project.contractorCompany"
                               autocomplete="off">
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
    export default {
        computed: {
            project() {
                return this.$store.getters.project;
            }
        },

        methods: {
            isValidFields() {
                if (!this.project.customerCompany) {
                    alert("Please enter the project customer.");

                    return false;
                }

                if (!this.project.contractorCompany) {
                    alert("Please enter the project contractor.");

                    return false;
                }

                return true;
            },

            nextStep() {
                if (!this.isValidFields()) {
                    return;
                }

                this.$router.push("/manager");
            },

            prevStep() {
                this.$router.push("/");
            },
        }
    }
</script>
