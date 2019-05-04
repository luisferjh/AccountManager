<template>
  <div class="row justify-content-center">
    <b-col md="5" class="bg-light">
      <h2 class="text-center mt-3">Login</h2>
      <b-form @submit="onSubmit" @reset="onReset" v-if="show">
        <b-form-group
          id="input-group-email"
          label="Email address:"
          label-for="input-email"        
        >
        <b-form-input
          id="input-email"
          v-model="form.email"
          type="email"
          required
          placeholder="Enter email"
        ></b-form-input>
        </b-form-group>

        <b-form-group id="input-group-password" label="Your Password:" label-for="input-password">
        <b-form-input
          type="password"
          id="input-password"
          v-model="form.password"
          required
          placeholder="Enter password"
        ></b-form-input>
        <b-alert show variant="danger" v-if="error">{{error}}</b-alert>
        </b-form-group>

        <div class="text-center">
            <b-button @click="getIn" class="mb-4" variant="primary">Enter</b-button>
        </div>    
      </b-form>
    </b-col>
  </div>  
</template>

<script>
import axios from 'axios'

export default {
  name:'Login',  
    data() {
      return {
        form: {
          email: '',
          password:''                        
        },
        error:null,
        show: true
      }
    },
    methods: {
      getIn(){
        let me = this;
        me.error=null;
        axios.post('https://localhost:44329/api/users/login',{											
					Password: this.form.password,					
					Email:this.form.email
				}).then(response =>{            												
          return response.data 
        })
        .then(data =>{
          this.$store.dispatch("saveToken", data.token)
          this.$router.push({name:'account'})
        })
        .catch(function(err){
          console.log(err.response)
          if (err.response.status == 400) {
            me.error = " Isn't a valid email"
          }else if(err.response.status == 404){
            me.error = "Don't exist user neither data are incorrect"
          }else{
            me.error = "happened an error"
          }
  					console.log(err);            
				});		
      },
      onSubmit(evt) {
        evt.preventDefault()
        alert(JSON.stringify(this.form))
      },
      onReset(evt) {
        evt.preventDefault()
        // Reset our form values
        this.form.email = ''       
        this.form.checked = []
        // Trick to reset/clear native browser form validation state
        this.show = false
        this.$nextTick(() => {
          this.show = true
        })
      }
    }
  }
</script>
