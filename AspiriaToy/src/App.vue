<template>
  <img alt="logo" style="width: 35%; height:35%;" src="./assets/logo-aspiria-twitter.webp">
  <AspiriaHome />
</template>

<script>
import AspiriaHome from './components/AspiriaHome.vue'

export default {
  name: 'App',
  components: {
      AspiriaHome
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
