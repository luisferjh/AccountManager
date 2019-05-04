import Vue from 'vue'
import Vuex from 'vuex'
import decode from 'jwt-decode'
import router from './router'

Vue.use(Vuex)

export default new Vuex.Store({
  //state no puede ser modificado directamente es de solo lectura
  // nuestros componentes van a emitir acciones desde la vista para 
  // modificar los valores de state
  state: {   
    token: null, //
    user: null,   //
    searchString:''
  },
  //la logica para cambiar el "state" es aqui
  mutations: {
    setToken(state,token){
      state.token = token
    },
    setUser(state,user){
      state.user = user
    }  
  },
  // Son similares a las mutaciones, la diferencia es que en lugar de mutar el 
  // estado las acciones realizan mutaciones
  actions: {
     //guardar el token que devuelva el backend
    saveToken({commit}, token){ // el commit nos permite realizar una mutacion o acceder al state
      commit("setToken", token)
      commit("setUser", decode(token))
      localStorage.setItem("token", token)
    },
    // verificar si el usuario ingreso las credenciales correctas y ya se ha generado un token      
    autoLogin({commit}){ 
      let token = localStorage.getItem("token")
      if (token) {
        commit("setToken", token)
        commit("setUser", decode(token))
      }
      router.push({name:'account'})
    },
    //eliminar el token registrado para el el user se loguee nuevamente
    exit({commit}) { 
      commit("setToken", null)
      commit("setUser", null)
      localStorage.removeItem("token")
      router.push({name: 'login'})
    }    
  }
})