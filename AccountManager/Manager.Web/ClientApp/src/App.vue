<template>
  <div id="app">
        <b-navbar toggleable="md" v-if="lloggedIn">                 
            <b-navbar-brand href="#" class="text-white title-app">Account Manager</b-navbar-brand>                                                            
            <b-nav-form  class="ml-auto">
              <b-form-input class="mr-sm-2" v-model="$store.state.searchString" placeholder="Search"></b-form-input>
              <b-button class="my-2 my-sm-0" @click="getOut" v-if="lloggedIn">
               <i class="fas fa-sign-out-alt"></i>
              </b-button>
              <b-button class="my-2 my-sm-0" :to="{name: 'login'}" v-else>
                <i class="fas fa-sign-in-alt"></i>
              </b-button>
            </b-nav-form>
        </b-navbar>   
    
    <b-container class="bv-example-row">     
      <!-- <router-view class="account">aa</router-view> -->
       <router-view/>
    </b-container>


  </div>
</template>

<script>


export default {
   name: "App", 
  data() {
    return {
      searchString:''
    }    
  },
  computed: {
    // este metodo va a obtener el user
   lloggedIn(){
      return this.$store.state.user;
    }    
  },
  created() {
    this.$store.dispatch("autoLogin")
    this.searchString = this.$store.state.searchString
  },
  methods:{
    getOut(){
      this.$store.dispatch('exit')
    }                  
  }        
}
</script>

<style lang="scss" src="./styles/global.scss"></style>